terraform {
  required_providers {
    azurerm = {
      source = "hashicorp/azurerm"
      # Root module should specify the maximum provider version
      # The ~> operator is a convenient shorthand for allowing only patch releases within a specific minor release.
      version = "~> 3.82.0"
    }
  }

  backend "azurerm" {
    resource_group_name  = "cicd-resource-group"
    storage_account_name = "cicdazurestorage"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
    client_id = "fd10a5c5-0e96-4fe2-8b0d-97c9c30d1db5"
    client_secret = "qc18Q~mYin5JXXRBOz7rNtkL8VZY~A_yCyi47aUG"
    tenant_id = "67d26f01-57b8-4b3c-b894-986813aa047a"
    subscription_id = "06db39db-61fb-47d6-b950-ad850ffd17cf"
  }
}

provider "azurerm" {
  features {}

  client_id = var.agent_client_id
  client_secret = var.agent_client_secret
  tenant_id = var.tenant_id
  subscription_id = var.subscription_id
}

resource "azurerm_resource_group" "resource_group" {
  name = "${var.project}-${var.environment}-resource-group"
  location = var.location
}

resource "azurerm_storage_account" "storage_account" {
  name = "${var.project}${var.environment}azurestorage"
  resource_group_name = azurerm_resource_group.resource_group.name
  location = var.location
  account_tier = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_application_insights" "application_insights" {
  name                = "${var.project}-${var.environment}-application-insights"
  location            = var.location
  resource_group_name = azurerm_resource_group.resource_group.name
  application_type    = "web"
}

resource "azurerm_service_plan" "app_service_plan" {
  name                = "${var.project}-${var.environment}-app-service-plan"
  resource_group_name = azurerm_resource_group.resource_group.name
  location            = var.location
  os_type             = "Linux"
  sku_name            = "S1"
}

resource "azurerm_linux_function_app" "function_app" {
  name                       = "${var.project}-${var.environment}-function-app"
  resource_group_name        = azurerm_resource_group.resource_group.name
  location                   = var.location
  service_plan_id        = azurerm_service_plan.app_service_plan.id
  app_settings = {
    "APPINSIGHTS_INSTRUMENTATIONKEY" = azurerm_application_insights.application_insights.instrumentation_key,
  }
  storage_account_name       = azurerm_storage_account.storage_account.name
  storage_account_access_key = azurerm_storage_account.storage_account.primary_access_key

  site_config {
    application_stack {
      dotnet_version = "8.0"
      use_dotnet_isolated_runtime = true
    }
  }
}

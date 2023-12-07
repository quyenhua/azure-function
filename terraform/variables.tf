variable "project" {
  type        = string
  description = "Project name"
  default     = "api"
}

variable "environment" {
  type        = string
  description = "Environment (dev / stage / prod)"
  default     = "dev"
}

variable "location" {
  type        = string
  description = "Azure region where to create resources."
  default     = "Australia Central"
}

variable "resource_group_name" {
  type        = string
  description = "Backend resource group name."
}

variable "account_name" {
  type        = string
  description = "Backend account name."
}

variable "agent_client_id" {
  type        = string
  description = "Azure agent client id."
}

variable "agent_client_secret" {
  type        = string
  description = "Azure agent client secret"
}

variable "subscription_id" {
  type        = string
  description = "Azure subscription id."
}

variable "tenant_id" {
  type        = string
  description = "Azure tenant id."
}

variable "tags" {
  type        = map(any)
  description = "Specifies a map of tags to be applied to the resources created."
}
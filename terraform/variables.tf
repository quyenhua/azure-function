variable "project" {
  type = string
  description = "Project name"
  default="api"
}

variable "environment" {
  type = string
  description = "Environment (dev / stage / prod)"
  default="dev"
}

variable "location" {
  type        = string
  description = "Azure region where to create resources."
  default="Australia Central"
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
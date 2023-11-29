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
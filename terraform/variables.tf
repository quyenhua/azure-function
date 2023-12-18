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

variable "tags" {
  type        = map(any)
  description = "Specifies a map of tags to be applied to the resources created."
}
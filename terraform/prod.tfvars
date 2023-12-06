project = "api"
environment = "prod"
location = "Australia Central"
resource_group_name="cicd-resource-group"
account_name="cicdazurestorage"
tags = {
  terraformDeployment = "true",
  GithubRepo          = "https://github.com/quyenhua/azure-function"
  Environment         = "PROD"
}
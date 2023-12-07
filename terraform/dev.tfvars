project             = "api"
environment         = "dev"
location            = "Australia Central"
resource_group_name = "cicd-resource-group"
account_name        = "cicdazurestorage"
tags = {
  terraformDeployment = "true"
  GithubRepo          = "https://github.com/quyenhua/azure-function"
  Environment         = "DEV"
}
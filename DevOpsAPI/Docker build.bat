echo on
docker build -f DevOpsAPI\Dockerfile --force-rm --tag devops-back .
@echo off
pause
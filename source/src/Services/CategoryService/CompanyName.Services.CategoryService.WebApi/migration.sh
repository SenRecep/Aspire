#!/bin/bash
MIGRATION_NAME=$1

if [ -z "$MIGRATION_NAME" ]; then
  echo "Error: Migration name is required"
  exit 1
fi

dotnet ef migrations add $MIGRATION_NAME --project ../CompanyName.Services.CategoryService.Persistence --context ApplicationWriteDbContext --output-dir EntityFrameworkCore/Migrations/ApplicationWrite

if [ $? -eq 0 ]; then
  echo "Migration '$MIGRATION_NAME' added successfully."
else
  echo "Failed to add migration '$MIGRATION_NAME'."
fi

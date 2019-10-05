EF: Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 3.0.0
x migration: Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.0.0

generare il DB: Update-Database (il data bse si chiama SgartTodoDb)

aggiungere una migration: Add-Migration -Name InitialMigration

rimuovere l'ultima miration: Remove-Migration


Remove-Migration; Add-Migration -Name InitialMigration ; Update-Database

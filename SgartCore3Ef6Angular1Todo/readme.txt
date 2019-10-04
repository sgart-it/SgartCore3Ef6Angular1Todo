https://code-maze.com/net-core-web-api-ef-core-code-first/

EF: Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 3.0.0
x migration: Install-Package Microsoft.EntityFrameworkCore.Tools -Version 3.0.0

generare il DB

Add-Migration -Name InitialMigration ; Update-Database

Remove-Migration
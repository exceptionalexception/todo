IF NOT EXISTS (
    SELECT name 
    FROM sys.databases 
    WHERE name = 'TodoAppDb'
)
CREATE DATABASE TodoAppDb;

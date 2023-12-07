USE TodoAppDb;

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Todos')
BEGIN
    CREATE TABLE Todos (
        TodoUId UNIQUEIDENTIFIER PRIMARY KEY,
        TodoText NVARCHAR(255) NOT NULL,
        DueDate DATETIME NOT NULL,
        CreatedDate DATETIME NOT NULL,
        IsComplete BIT NOT NULL,
        ParentTodoUId UNIQUEIDENTIFIER NULL,
        FOREIGN KEY (ParentTodoUId) REFERENCES Todos(TodoUId)
    );

    CREATE INDEX idx_Todos_TodoUId ON Todos (TodoUId);
END

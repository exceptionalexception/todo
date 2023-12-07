INSERT INTO Todos (TodoUId, TodoText, DueDate, CreatedDate, IsComplete)
VALUES (NEWID(), 'Clean the house.', GETDATE(), GETDATE(), 1),
       (NEWID(), 'Do the laundry.', DATEADD(day, -2, CAST(GETDATE() AS date)), GETDATE(), 0),
       (NEWID(), 'Sleep.', DATEADD(day, -30, CAST(GETDATE() AS date)), GETDATE(), 0),
       (NEWID(), 'Take kids to playground.', DATEADD(day, 1, CAST(GETDATE() AS date)), GETDATE(), 0),
       (NEWID(), 'Dance.', DATEADD(day, -1, CAST(GETDATE() AS date)), GETDATE(), 1),
       (NEWID(), 'Read.', GETDATE(), GETDATE(), 0),
       (NEWID(), 'Plant more trees.', GETDATE(), GETDATE(), 0);

INSERT INTO Todos (TodoUId, TodoText, DueDate, CreatedDate, IsComplete)
VALUES ('3fa85f64-5717-4562-b3fc-2c963f66afa6', 'Replace old outlets with GFCI.', GETDATE(), GETDATE(), 1);

INSERT INTO Todos (TodoUId, TodoText, DueDate, CreatedDate, ParentTodoUId, IsComplete)
VALUES (NEWID(), 'Go to Lowes and buy GFCI outlet.', GETDATE(), GETDATE(), '3fa85f64-5717-4562-b3fc-2c963f66afa6', 1),
       (NEWID(), 'Find your tools.', DATEADD(day, -2, CAST(GETDATE() AS date)), GETDATE(), '3fa85f64-5717-4562-b3fc-2c963f66afa6', 0),
       (NEWID(), 'Replace the outlets.', GETDATE(), GETDATE(), '3fa85f64-5717-4562-b3fc-2c963f66afa6', 0);
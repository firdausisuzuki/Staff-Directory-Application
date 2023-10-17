-- 1. Prerequisites - Please install SQL Server Express before proceeding to the next step.

-- 2. In the server explorer, right click on the Data Connections, and Add Connection...
--    Server name = .\sqlexpress
--    Enter a database name = staffdirectory

-- 3. Right-click on the tables and click New Query
--    Run the query below to create the table and to insert a few data for testing

-- ** Make sure to check the appsettings.json for the connectionString
-- ** You can find the connectionString by left-clicking the database created in the Server Explorer under the Properties menu

CREATE TABLE staffs (
    id INT NOT NULL PRIMARY KEY IDENTITY,
    fname VARCHAR (100) NOT NULL,
    lname VARCHAR (100) NOT NULL,
    gender VARCHAR (100) NOT NULL,
    dateofbirth DATE NOT NULL,
    position VARCHAR (20) NOT NULL,
    department VARCHAR(100) NOT NULL,
    created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP
);

INSERT INTO staffs (fname, lname, gender, dateofbirth, position, department)
VALUES
('Bill', 'Gates', 'Male', '1955-10-28', 'CEO', 'Management'),
('Elon', 'Musk', 'Male', '1971-06-28', 'CEO', 'Management'),
('Will', 'Smith', 'Male', '1968-09-25', 'HR Executive', 'HR'),
('Bob', 'Marley', 'Male', '1998-04-17', 'IT Executive', 'IT'),
('Cristiano', 'Ronaldo', 'Male', '1985-02-05', 'Customer Support', 'IT');


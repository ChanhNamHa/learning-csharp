CREATE TABLE X_Acc (
    ID VARCHAR(50) PRIMARY KEY,
    TK NVARCHAR(MAX),
    MK NVARCHAR(MAX),
    AccNew BIT, -- 1: New, 0: 7 Days
    IsSold BIT DEFAULT 0,
    Created_Date DATETIME DEFAULT GETDATE()
);

CREATE TABLE [Transaction] (
    ID VARCHAR(50) PRIMARY KEY,
    User_ID BIGINT,
    Payment_Amount BIGINT,
    Quantity INT,
    AccType BIT,
    Status NVARCHAR(50) DEFAULT 'Pending',
    Created_Date DATETIME DEFAULT GETDATE()
);

CREATE TABLE X_Acc_Sold (
    ID_X_Acc VARCHAR(50),
    Transaction_ID VARCHAR(50),
    PRIMARY KEY (ID_X_Acc, Transaction_ID)
);
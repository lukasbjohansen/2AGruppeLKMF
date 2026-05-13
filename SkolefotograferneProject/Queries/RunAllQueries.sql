-- ============================================================
-- VIBECODED --
-- RunAll.sql  –  Master setup script
-- Run with sqlcmd or SSMS in SQLCMD mode
--
-- In SSMS:   Query menu → SQLCMD Mode, then execute this file
-- In sqlcmd: sqlcmd -S <server> -d <database> -i RunAll.sql
-- ============================================================

PRINT 'Step 1/4 – Dropping existing tables...';
:r DropTables.sql

PRINT 'Step 2/4 – Creating tables...';
:r CreateTables.sql

PRINT 'Step 3/4 – Inserting postal codes / cities...';
:r InsertCities.sql

PRINT 'Step 4/4 – Inserting test values...';
:r InsertTestValues.sql

PRINT 'Done! All steps completed successfully.';
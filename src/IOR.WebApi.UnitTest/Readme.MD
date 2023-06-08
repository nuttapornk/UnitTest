# Test Case Medication incident

## 001 Common function test
1. Check date value,Do not exceed the current data (true).
1. Check date value,Do not exceed the current data (fals).
1. Get risk team
4. Get full location text.
5. Get event effect name.
6. Get org full id.
7. Get org full name.
8. Get user name
9. Get document running number. 

## 002 Normal test case 
### 01 Create incident report success result return Ok(200)
1. Create incident report with real user and existing in the database .
1. Create incident report with real user and Nonexistent in the database (new user).

## 003 Alternative test case 

## 004 Exception test case

### 01 Create incident report fail result return Bad Request(400)
1. Data not valid(Not pass all data).
1. Data not valid(Not pass location,severity,Med Eff type)
1. User does not exist.
1. Location data existing in the database.
1. Incident data wrong date.
1. Incident date more than current date.
1. Incident type not pass value.
1. Incident code not pass value.
1. Category differrent between incident type and incident code.
1. Severity wrong value( not between A and I).
1. MedEffType wrong value value not between(1 and 2).
--step 1
SELECT COUNT(*) AS Patients
FROM Patient;
SELECT COUNT(*) AS Encounters
FROM Encounter;
SELECT COUNT(*) AS Diagnoses
FROM Diagnosis;
SELECT COUNT(*) AS Claims
FROM Claim;

--step 2
INSERT INTO Patient
(
Mrn,
FullName,
DateOfBirth,
Gender,
City,
IsActive
)
VALUES
(
'MRN999999',
'Rahul Verma',
'1985-06-15',
'M',
'Hyderabad',
1
);
--step 3
select * from Patient where Mrn = 'MRN999999';

--step 4
INSERT INTO Encounter
(
PatientId,
ProviderId,
DepartmentId,
AdmitDate,
DischargeDate,
EncounterType
)
SELECT
1007,
1,
1,
DATEADD(DAY,-v.number,GETDATE()),
GETDATE(),
'Outpatient'
FROM master..spt_values v
WHERE v.type = 'P'
AND v.number < 500;

--step 5
SELECT COUNT(*) AS EncounterCount
FROM Encounter
WHERE PatientId = 1007;

--step 6
INSERT INTO Diagnosis
(
EncounterId,
IcdCode,
Description,
DiagnosedOn
)
SELECT
EncounterId,
'I10',
'Hypertension',
GETDATE()
FROM Encounter
WHERE PatientId = 1007;

--step 7
SELECT COUNT(*) AS DiagnosisCount
FROM Diagnosis d
INNER JOIN Encounter e
ON d.EncounterId = e.EncounterId
WHERE e.PatientId = 1007;

--STEP 8
INSERT INTO Claim
(
EncounterId,
InsuranceId,
BilledAmount,
ReimbursedAmt,
Status
)
SELECT
EncounterId,
1,
15000,
12000,
'Paid'
FROM Encounter
WHERE PatientId = 1007;

--STEP 9 - VERIFY CLAIMS
SELECT COUNT(*) AS ClaimCount
FROM Claim c
INNER JOIN Encounter e
ON c.EncounterId = e.EncounterId
WHERE e.PatientId = 1007;

 
-- STEP 10 - CREATE EVEN MORE DATA
INSERT INTO Encounter
(
PatientId,
ProviderId,
DepartmentId,
AdmitDate,
DischargeDate,
EncounterType
)
SELECT
p.PatientId,
1,
1,
DATEADD(DAY,-ABS(CHECKSUM(NEWID())) % 365,GETDATE()),
GETDATE(),
'Outpatient'
FROM Patient p
CROSS JOIN
(
SELECT TOP 20
ROW_NUMBER() OVER(ORDER BY (SELECT NULL)) AS N
FROM sys.objects
) x;

-- STEP 11 - CREATE DIAGNOSES FOR NEW ENCOUNTERS
INSERT INTO Diagnosis
(
EncounterId,
IcdCode,
Description,
DiagnosedOn
)
SELECT
EncounterId,
'I10',
'Hypertension',
GETDATE()
FROM Encounter
WHERE EncounterId NOT IN
(
SELECT DISTINCT EncounterId
FROM Diagnosis
);
-- STEP 12 - CREATE CLAIMS FOR NEW ENCOUNTERS
INSERT INTO Claim
(
EncounterId,
InsuranceId,
BilledAmount,
ReimbursedAmt,
Status
)
SELECT
EncounterId,
1,
15000,
12000,
'Paid'
FROM Encounter
WHERE EncounterId NOT IN
(
SELECT DISTINCT EncounterId
FROM Claim
);
-- FINAL VERIFICATION
SELECT COUNT(*) AS Patients
FROM Patient;
SELECT COUNT(*) AS Encounters
FROM Encounter;
SELECT COUNT(*) AS Diagnoses
FROM Diagnosis;
SELECT COUNT(*) AS Claims
FROM Claim;

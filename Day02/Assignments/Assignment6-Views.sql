CREATE OR ALTER VIEW vw_Clinical
AS
SELECT 
    e.EncounterId,
    e.PatientId,
    e.AdmitDate,
    e.EncounterType,
    d.IcdCode,
    d.Description
FROM Encounter e
INNER JOIN Diagnosis d ON e.EncounterId = d.EncounterId;

CREATE OR ALTER VIEW vw_Billing
AS
SELECT 
    c.ClaimId,
    c.EncounterId,
    c.BilledAmount,
    ISNULL(c.ReimbursedAmt,0) AS ReimbursedAmount,
    (c.BilledAmount - ISNULL(c.ReimbursedAmt,0)) AS RevenueLeakage,
    i.Payer
FROM Claim c
INNER JOIN Insurance i ON c.InsuranceId = i.InsuranceId;

CREATE OR ALTER VIEW vw_Analytics
AS
SELECT 
    CASE 
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) < 18 THEN 'Child'
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) BETWEEN 18 AND 64 THEN 'Adult'
        ELSE 'Senior'
    END AS AgeGroup,
    e.EncounterType,
    COUNT(*) AS EncounterCount
FROM Patient p
INNER JOIN Encounter e ON p.PatientId = e.PatientId
GROUP BY 
    CASE 
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) < 18 THEN 'Child'
        WHEN DATEDIFF(YEAR, p.DateOfBirth, GETDATE()) BETWEEN 18 AND 64 THEN 'Adult'
        ELSE 'Senior'
    END,
    e.EncounterType;

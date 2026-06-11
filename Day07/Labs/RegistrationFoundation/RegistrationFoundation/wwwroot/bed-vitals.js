// =====================================================
// Smart Hospital Bed Monitoring Dashboard
// Demonstrates:
// Arrays, Objects, Conditions, Loops, DOM, Business Rules
// =====================================================


// -----------------------------
// BED DATA (Mock backend)
// -----------------------------
let beds = [
    {
        bedNumber: 1,
        patientName: "Anita Sharma",
        heartRate: 78,
        spo2: 98
    },
    {
        bedNumber: 2,
        patientName: "Ramesh Kumar",
        heartRate: 110,
        spo2: 92
    },
    {
        bedNumber: 3,
        patientName: "Sunita Verma",
        heartRate: 130,
        spo2: 85
    },
    {
        bedNumber: 4,
        patientName: "Amit Singh",
        heartRate: 82,
        spo2: 96
    },
    {
        bedNumber: 5,
        patientName: "Neha Joshi",
        heartRate: 105,
        spo2: 90
    }
];


// -----------------------------
// FUNCTION: Determine bed status
// -----------------------------
function getBedStatus(heartRate, spo2) {

    // Critical condition
    if (heartRate > 120 || spo2 < 90) {
        return "critical";
    }

    // Observation condition
    if (heartRate > 100 || spo2 < 95) {
        return "observation";
    }

    // Otherwise stable
    return "stable";
}


// -----------------------------
// FUNCTION: Render beds
// -----------------------------
function renderBeds() {

    let container = document.getElementById("bedContainer");
    container.innerHTML = "";

    // Loop through beds
    for (let i = 0; i < beds.length; i++) {

        let bed = beds[i];

        // Create bed div
        let bedDiv = document.createElement("div");
        bedDiv.classList.add("bed");

        // Determine status using vitals
        let status = getBedStatus(bed.heartRate, bed.spo2);
        bedDiv.classList.add(status);

        // Bed label
        bedDiv.innerHTML = `
        <span class="critical-label">Bed ${bed.bedNumber}</span>
        <div class="tooltip">
            <b>Patient:</b> ${bed.patientName}<br>
            <b>Heart Rate:</b> ${bed.heartRate} bpm<br>
            <b>SpO₂:</b> ${bed.spo2} %
        </div>
        `;

        container.appendChild(bedDiv);
    }
}


// -----------------------------
// INITIAL LOAD
// -----------------------------
renderBeds();

// ===================================================
// Hospital Bed Availability Dashboard
// Demonstrates:
// - Arrays
// - Objects
// - Loops
// - Conditions
// - DOM manipulation
// ===================================================


// -----------------------------
// BED DATA (Mock backend data)
// -----------------------------
let beds = [
    { bedNumber: 1, isOccupied: false },
    { bedNumber: 2, isOccupied: true },
    { bedNumber: 3, isOccupied: false },
    { bedNumber: 4, isOccupied: true },
    { bedNumber: 5, isOccupied: false },
    { bedNumber: 6, isOccupied: false },
    { bedNumber: 7, isOccupied: true },
    { bedNumber: 8, isOccupied: false },
    { bedNumber: 9, isOccupied: true },
    { bedNumber: 10, isOccupied: false },
    { bedNumber: 11, isOccupied: false },
    { bedNumber: 12, isOccupied: false }

];


// -----------------------------
// FUNCTION: Render beds on screen
// -------

function renderBeds() {
    let container = document.getElementById("bedContainer");
    let summary = document.getElementById("summary");

    let c = 0;

    container.innerHTML = "";

    // Loop through all beds
    for (let i = 0; i < beds.length; i++) {
        let bed = beds[i];
        let bedDiv = document.createElement("div");
        bedDiv.classList.add("bed");

        if (bed.isOccupied) {
            bedDiv.classList.add("occupied");
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nOccupied";
            c++;
        } else {
            bedDiv.classList.add("available");
            bedDiv.innerText = "Bed " + bed.bedNumber + "\nAvailable";
        }
        if (!bed.isOccupied) {
            bedDiv.onclick = function () {
                bed.isOccupied = !bed.isOccupied;
                renderBeds();
            };
        }
        container.appendChild(bedDiv);
    }

    summary.innerText = "Occupied: " + c + "/"  + beds.length;
}

// -----------------------------
// INITIAL LOAD
// -----------------------------
renderBeds();

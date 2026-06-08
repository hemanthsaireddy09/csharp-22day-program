<script setup>
import { ref, onMounted, watch } from 'vue'

const city = ref('Pune')
const isActive = ref(true)
const search = ref('')  
const patients = ref([])
const cities = ref([])
// Function to load patients with filters
async function loadCities(){
  const response = await fetch(
    'http://localhost:5159/api/cities'
  )
  cities.value = await response.json()
}
async function loadPatients() {
  const params = new URLSearchParams();

  if (city.value && city.value !== "All") {
    params.append("city", city.value);
  }

  if (isActive.value && isActive.value !== "All") {
    params.append("isActive", isActive.value);
  }

  if (search.value) {
    params.append("search", search.value);
  }

  const response = await fetch(`http://localhost:5159/api/patients?${params.toString()}`);
  patients.value = await response.json();
}

// Load initial data
onMounted(async () => {
  await loadCities()
  await loadPatients()
})


</script>

<template>
  <h1>CareBridge Patients</h1>
  <div class="filters">
  <!-- Search bar -->
  <input
    v-model="search"
    placeholder="Search by name"
    style="margin-bottom:10px; display:flex; justify-content: center;"
  />
    <!-- City dropdown -->
  <select v-model="city">
    <option value="">All Cities</option>
    <option v-for="c in cities" :key="c" :value="c">
      {{ c }}
    </option>
  </select>

  <!-- IsActive filter -->
  <select v-model="isActive">
    <option :value="null">All</option>
    <option :value="true">Active</option>
    <option :value="false">Inactive</option>
  </select>
      <button @click="loadPatients">Search</button>
  </div>
   <p>
    Showing {{ patients.length }} patient<span v-if="patients.length !== 1">s</span>
  </p>
  <table border="1">
    <tr>
      <th>Patient Id</th>
      <th>Full Name</th>
      <th>City</th>
      <th>Is Active</th>
    </tr>

    <tr v-for="p in patients" :key="p.patientId">
      <td>{{ p.patientId }}</td>
      <td>{{ p.fullName }}</td>
      <td>{{ p.city }}</td>
      <td>{{ p.isActive }}</td>
    </tr>
  </table>
</template>

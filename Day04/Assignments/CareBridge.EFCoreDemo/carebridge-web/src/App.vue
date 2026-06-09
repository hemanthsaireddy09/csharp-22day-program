<script setup>
import { ref, onMounted, watch } from 'vue'

const search = ref('')  
const department_load = ref([])

async function loadDepartments() {

  const response = await fetch('http://localhost:5159/api/analytics/department-load');
  department_load.value = await response.json();
}

// Load initial data
onMounted(loadDepartments())


</script>

<template>
  <h1>CareBridge Department Load</h1>
  
 
  <table border="1">
     <tr>
          <th>Department</th>
          <th>Inpatient</th>
          <th>Outpatient</th>
          <th>ED</th>
          <th>Total</th>
        </tr>

<tr 
  v-for="(dept, index) in department_load" 
  :key="dept.departmentName"
  :class="{ highlight: index === 0 }"
>          <td>{{ dept.departmentName }}</td>
          <td>{{ dept.inpatient }}</td>
          <td>{{ dept.outpatient }}</td>
          <td>{{ dept.ed }}</td>
          <td>{{ dept.total }}</td>
        </tr>
  </table>
</template>
<style>
  .highlight {
  background-color: rgb(238, 3, 3);
  font-weight: bolder;
}

</style>
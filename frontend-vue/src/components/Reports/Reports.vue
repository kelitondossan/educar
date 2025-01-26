<template>
  <v-container fluid class="pa-6">
    <h1 class="dashboard-title">Student Reports</h1>
    
    <v-toolbar flat>
      <v-toolbar-title>Students List</v-toolbar-title>
      <v-divider class="mx-4" inset vertical></v-divider>
      <v-spacer></v-spacer>
      <v-text-field
        v-model="search"
        append-icon="mdi-magnify"
        label="Search"
        single-line
        hide-details
      ></v-text-field>
    </v-toolbar>

    <div class="table-wrapper">
      <v-simple-table class="professional-table styled-simple-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Email</th>
            <th>CPF</th>
            <th></th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="student in paginatedStudents" :key="student.id">
            <td>{{ student.id }}</td>
            <td>{{ student.name }}</td>
            <td>{{ student.email }}</td>
            <td>{{ formatCPF(student.cpf) }}</td>
            <td>
              <v-btn color="primary" @click="printReport(student)" small>
                <v-icon left>mdi-printer</v-icon> Print
              </v-btn>
            </td>
          </tr>
          <tr v-if="paginatedStudents.length === 0">
            <td colspan="5">
              <v-alert type="warning" outlined>
                No students found.
              </v-alert>
            </td>
          </tr>
        </tbody>
      </v-simple-table>
    </div>

    <v-row justify="end" class="mt-4">
      <v-col cols="12" sm="6" md="4">
        <v-pagination
          v-model="currentPage"
          :length="totalPages"
          :total-visible="7"
        ></v-pagination>
      </v-col>
    </v-row>
    
    <v-snackbar
      v-model="showSnackbar"
      :color="snackbarColor"
      top
      right
      timeout="3000"
    >
      {{ snackbarMessage }}
      <v-btn color="white" text @click="showSnackbar = false">
        Close
      </v-btn>
    </v-snackbar>
  </v-container>
</template>

<script>
import axios from 'axios';
import { ref, onMounted, computed, watch } from 'vue';

export default {
  name: 'Reports',
  setup() {
    const headers = [
   ,
    ];

    const students = ref([]);
    const search = ref('');

    const showSnackbar = ref(false);
    const snackbarMessage = ref('');
    const snackbarColor = ref('');

    const currentPage = ref(1);
    const itemsPerPage = ref(10);

    const fetchStudents = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Students');
        console.log('Students data:', response.data);
        students.value = response.data;
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching the list of students.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };

    const formatCPF = (cpf) => {
      if (!cpf) return '';
      const cpfStr = cpf.toString().replace(/\D/g, '');
      if (cpfStr.length !== 11) return cpf;
      return cpfStr.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
    };

    const printReport = (student) => {
      const printWindow = window.open('', '', 'width=800,height=600');
      
      const reportContent = `
        <html>
          <head>
            <title>Student Report</title>
            <style>
              body { font-family: Arial, sans-serif; padding: 20px; }
              h1 { color: #1976d2; }
              table { width: 100%; border-collapse: collapse; margin-top: 20px; }
              th, td { border: 1px solid #dddddd; text-align: left; padding: 8px; }
              th { background-color: #f2f2f2; }
            </style>
          </head>
          <body>
            <h1>Student Report</h1>
            <table>
              <tr>
                <th>ID</th>
                <td>${student.id}</td>
              </tr>
              <tr>
                <th>Name</th>
                <td>${student.name}</td>
              </tr>
              <tr>
                <th>Email</th>
                <td>${student.email}</td>
              </tr>
              <tr>
                <th>CPF</th>
                <td>${formatCPF(student.cpf)}</td>
              </tr>
              <!-- Add more fields as necessary -->
            </table>
          </body>
        </html>
      `;

      printWindow.document.write(reportContent);
      printWindow.document.close();

      printWindow.onload = () => {
        printWindow.focus();
        printWindow.print();
        printWindow.close();
      };
    };

    const filteredStudents = computed(() => {
      if (search.value) {
        const searchLower = search.value.toLowerCase();
        return students.value.filter((student) =>
          Object.values(student).some((value) =>
            String(value).toLowerCase().includes(searchLower)
          )
        );
      }
      return students.value;
    });

    const totalPages = computed(() => {
      return Math.ceil(filteredStudents.value.length / itemsPerPage.value);
    });

    const paginatedStudents = computed(() => {
      const start = (currentPage.value - 1) * itemsPerPage.value;
      const end = start + itemsPerPage.value;
      return filteredStudents.value.slice(start, end);
    });

    watch(search, () => {
      currentPage.value = 1; 
    });

    onMounted(() => {
      fetchStudents();
    });

    return {
      headers,
      students,
      search,
      printReport,
      formatCPF, 
      showSnackbar,
      snackbarMessage,
      snackbarColor,
      currentPage,
      totalPages,
      paginatedStudents,
    };
  },
};
</script>
<style scoped>
.dashboard-title {
  margin-bottom: 2rem;
  text-align: center;
  color: #1976d2;
}
.stat-card {
  display: flex;
  align-items: center;
  border-radius: 12px;
}
.stat-icon {
  font-size: 3rem;
}
.stat-label {
  font-size: 1rem;
  color: #757575;
}
.stat-value {
  font-size: 1.5rem;
  font-weight: bold;
}
.primary {
  color: #ffffff !important;
  background-color: #1976d2 !important;
}
.success {
  color: #ffffff !important;
  background-color: #4caf50 !important;
}
.info {
  color: #ffffff !important;
  background-color: #2196f3 !important;
}
.warning {
  color: #ffffff !important;
  background-color: #ff9800 !important;
}
.ml-4 {
  margin-left: 1rem;
}
.chart-card {
  height: 400px;
}
@media (max-width: 600px) {
  .dashboard-title {
    font-size: 1.5rem;
  }
  .stat-label {
    font-size: 0.9rem;
  }
  .stat-value {
    font-size: 1.2rem;
  }
  .chart-card {
    height: 300px;
  }
}


.table-wrapper {
  width: 100%;
  overflow-x: auto; 
}

.styled-simple-table thead {
  background-color: #1976d2 !important; 
  color: #ffffff !important; 
  text-align: left; 
}

.styled-simple-table tbody td {
  color: #424242; 
  padding: 12px; 
  border-bottom: 1px solid #e0e0e0; 
  width: 100rem;

}


.styled-simple-table .actions {
  text-align: center;
}


.styled-simple-table tbody tr:nth-child(odd) {
  background-color: #f9f9f9;
}


.styled-simple-table tbody tr:hover {
  background-color: #e3f2fd;
}


.v-btn {
  transition: background-color 0.3s;
  margin-right: 4px; 
}

.v-btn:hover {
  opacity: 0.8;
}


.pa-6 {
  padding: 2rem !important;
}
.professional-card {
  border-radius: 12px;
}
.white--text {
  color: #ffffff !important;
}
.professional-table {
  border-radius: 8px;
  background-color: #ffffff;
  width: 100%; 
  table-layout: fixed; 
}


.v-dialog__content {
  background-color: #ffffff;
}
.v-alert {
  border-radius: 8px;
}
.v-snackbar {
  max-width: 300px;
}

@media (max-width: 600px) {
  .v-card-title {
    flex-direction: column;
    align-items: flex-start;
    text-align: center;
  }

  .v-btn.secondary {
    width: 100%;
    margin-top: 10px;
  }

  .styled-simple-table thead th {
    font-size: 0.9rem;
  }

  .styled-simple-table tbody td {
    font-size: 0.9rem;
  }
}


.v-text-field {
  max-width: 300px;
}
</style>

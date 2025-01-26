<template>
  <v-container fluid class="pa-6">
    <h1 class="dashboard-title">Dashboard</h1>
    <v-row>
      <!-- Total Students Card -->
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card" elevation="4">
          <v-card-title class="d-flex align-center">
            <v-icon large class="stat-icon primary">mdi-account-group</v-icon>
            <div class="ml-4">
              <div class="stat-label">Total Students</div>
              <div class="stat-value">{{ totalStudents }}</div>
            </div>
          </v-card-title>
        </v-card>
      </v-col>

      <!-- New Students This Month Card (No Functionality for Now) -->
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card" elevation="4">
          <v-card-title class="d-flex align-center">
            <v-icon large class="stat-icon success">mdi-account-plus</v-icon>
            <div class="ml-4">
              <div class="stat-label">New Students This Month</div>
              <div class="stat-value">{{ newStudents }}</div>
            </div>
          </v-card-title>
        </v-card>
      </v-col>
      
      <!-- Total Courses Card (No Functionality for Now) -->
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card" elevation="4">
          <v-card-title class="d-flex align-center">
            <v-icon large class="stat-icon info">mdi-book-open-page-variant</v-icon>
            <div class="ml-4">
              <div class="stat-label">Total Courses</div>
              <div class="stat-value">{{ totalCourses }}</div>
            </div>
          </v-card-title>
        </v-card>
      </v-col>
<!-- Active Instructors Card (No Functionality for Now) -->
      <v-col cols="12" sm="6" md="3">
        <v-card class="stat-card" elevation="4">
          <v-card-title class="d-flex align-center">
            <v-icon large class="stat-icon warning">mdi-account-tie</v-icon>
            <div class="ml-4">
              <div class="stat-label">Active Instructors</div>
              <div class="stat-value">{{ activeInstructors }}</div>
            </div>
          </v-card-title>
        </v-card>
      </v-col>
    </v-row>

    <!-- Enrollment Trends Chart (for Datas Mock Now) -->
    <v-row class="mt-8">
      <v-col cols="12">
        <v-card class="chart-card" elevation="4">
          <v-card-title>
            <span class="text-h6">Student Enrollment Trends</span>
          </v-card-title>
          <v-card-text>
            <Line :chart-data="enrollmentData" :options="chartOptions" />
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Snackbar for Error Notifications -->
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
import { ref, onMounted } from 'vue';
import { Line } from 'vue-chartjs';
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  LineElement,
  CategoryScale,
  LinearScale,
  PointElement,
} from 'chart.js';

ChartJS.register(Title, Tooltip, Legend, LineElement, CategoryScale, LinearScale, PointElement);

export default {
  name: 'Dashboard',
  components: {
    Line,
  },
  setup() {
    const totalStudents = ref(0);
    const newStudents = ref(0); 
    const totalCourses = ref(0); 
    const activeInstructors = ref(0);
    const enrollmentData = ref({
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
      datasets: [
        {
          label: 'Enrollments',
          backgroundColor: '#1976d2',
          borderColor: '#1976d2',
          data: [10, 20, 15, 25, 30, 20], // Dados mock
          fill: false,
        },
      ],
    });
    const chartOptions = ref({
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'top',
        },
        title: {
          display: false,
          text: 'Student Enrollment Trends',
        },
      },
    });

    // Snackbar 
    const showSnackbar = ref(false);
    const snackbarMessage = ref('');
    const snackbarColor = ref('');

    
    const fetchTotalStudents = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Students');
        totalStudents.value = response.data.length;
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching total students.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };

      /**
     * commented functions because the routes are not yet implemented
     */
      /*
    const fetchNewStudents = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Students/NewThisMonth');
        newStudents.value = response.data.length;
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching new students.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };

    const fetchTotalCourses = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Courses');
        totalCourses.value = response.data.length;
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching total courses.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };

    const fetchActiveInstructors = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Instructors/Active');
        activeInstructors.value = response.data.length;
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching active instructors.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };

    const fetchEnrollmentTrends = async () => {
      try {
        const response = await axios.get('http://localhost:5000/api/Students/EnrollmentTrends');
        enrollmentData.value = {
          labels: response.data.labels,
          datasets: [
            {
              label: 'Enrollments',
              backgroundColor: '#1976d2',
              borderColor: '#1976d2',
              data: response.data.data,
              fill: false,
            },
          ],
        };
      } catch (error) {
        console.error(error);
        snackbarMessage.value = 'Error fetching enrollment trends.';
        snackbarColor.value = 'red';
        showSnackbar.value = true;
      }
    };
    */

    onMounted(() => {
      fetchTotalStudents();
      //The functions below are commented because the routes do not yet exist 
      // //Decommend them when you are ready to implement
      // fetchNewStudents();
      // fetchTotalCourses();
      // fetchActiveInstructors();
      // fetchEnrollmentTrends();
    });

    return {
      totalStudents,
      newStudents,
      totalCourses,
      activeInstructors,
      enrollmentData,
      chartOptions,
      showSnackbar,
      snackbarMessage,
      snackbarColor,
    };
  },
};
</script>

<style scoped>
.pa-6 {
  padding: 2rem !important;
}

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
</style>

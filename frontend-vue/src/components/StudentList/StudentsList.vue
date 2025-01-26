<template>
  <v-container fluid class="pa-6">
    <v-card elevation="12" class="professional-card">
      <v-card-title class="primary white--text d-flex justify-space-between align-center">
        <span class="text-h5">Students List</span>
        <v-btn color="secondary" dark @click="goToCreate" elevation="2">
          <v-icon left>mdi-plus</v-icon>
          Create New Student
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row class="my-4" align="center">
          <v-col cols="12" sm="6" md="4">
            <v-text-field
              v-model="search"
              label="Search"
              append-icon="mdi-magnify"
              clearable
              outlined
              dense
            ></v-text-field>
          </v-col>
        </v-row>

        <div class="table-wrapper">
          <v-simple-table class="professional-table styled-simple-table">
            <thead>
              <tr>
                <th>RA</th>
                <th>Name</th>
                <th>CPF</th>
                <th>Email</th>
                <th class="actions-header"></th>
              </tr>
            </thead>

            <tbody>
              <tr v-for="student in paginatedStudents" :key="student.id">
                <td>{{ student.ra }}</td>
                <td>{{ student.name }}</td>
                <td>{{ student.cpf }}</td>
                <td>{{ student.email }}</td>
                <td class="actions">
                  <v-btn color="blue" @click="editStudent(student.id)" small>
                    Edit
                  </v-btn>
                  <v-btn color="red" @click="promptDeleteStudent(student.id)" small>
                    Delete
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
      </v-card-text>
    </v-card>

    <v-dialog v-model="showConfirmDialog" max-width="500">
      <v-card>
        <v-card-title class="headline primary white--text">
          Confirm Deletion
        </v-card-title>
        <v-card-text>
          Are you sure you want to delete this student?
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="green darken-1" text @click="cancelDelete">
            No
          </v-btn>
          <v-btn color="red darken-1" text @click="confirmDelete">
            Yes
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar
      v-model="showSnackbar"
      :color="snackbarColor"
      top
      right
      timeout="1000"
    >
      {{ snackbarMessage }}
      <v-btn color="white" text @click="showSnackbar = false">
        Close
      </v-btn>
    </v-snackbar>
  </v-container>
</template>

<script>
import axios from "axios";

export default {
  name: "StudentsList",
  data() {
    return {
      students: [],
      search: "",
      itemsPerPage: 10,
      currentPage: 1,
      totalItems: 0,

      showConfirmDialog: false,
      studentToDelete: null,

      showSnackbar: false,
      snackbarMessage: "",
      snackbarColor: "",

      isLoading: false,

      footerProps: {
        "items-per-page-options": [10, 20, 50],
        showFirstLastPage: true,
        showCurrentPage: true,
      },
    };
  },
  computed: {
    filteredStudents() {
      if (this.search) {
        const searchLower = this.search.toLowerCase();
        return this.students.filter((student) =>
          Object.values(student).some((value) =>
            String(value).toLowerCase().includes(searchLower)
          )
        );
      }
      return this.students;
    },
    totalPages() {
      return Math.ceil(this.filteredStudents.length / this.itemsPerPage);
    },
    paginatedStudents() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredStudents.slice(start, end);
    },
  },
  watch: {
    search() {
      this.currentPage = 1;
    },
    totalPages(newTotal) {
      if (this.currentPage > newTotal) {
        this.currentPage = newTotal || 1;
      }
    },
  },
  created() {
    this.fetchStudents();
  },
  methods: {
    fetchStudents() {
      this.isLoading = true;
      axios
        .get("http://localhost:5000/api/Students")
        .then((response) => {
          this.students = response.data.map((student) => ({
            ...student,
            cpf: this.formatCPF(student.cpf || ""),
          }));
          this.totalItems = this.students.length;
        })
        .catch((error) => {
          console.error(error);
          this.showSnackbar = true;
          this.snackbarMessage = "Error fetching students list.";
          this.snackbarColor = "red";
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    formatCPF(value) {
      let cpf = value.replace(/\D/g, "");
      cpf = cpf.substring(0, 11);

      if (cpf.length > 9) {
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, "$1.$2.$3-$4");
      } else if (cpf.length > 6) {
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{0,3})/, "$1.$2.$3");
      } else if (cpf.length > 3) {
        cpf = cpf.replace(/(\d{3})(\d{0,3})/, "$1.$2");
      }

      return cpf;
    },

    goToCreate() {
      this.$router.push({ name: "CreateStudent" });
    },

    editStudent(id) {
      this.$router.push({ name: "EditStudent", params: { id } });
    },

    promptDeleteStudent(id) {
      this.studentToDelete = id;
      this.showConfirmDialog = true;
    },

    cancelDelete() {
      this.showConfirmDialog = false;
      this.studentToDelete = null;
    },

    confirmDelete() {
      this.showConfirmDialog = false;
      if (this.studentToDelete) {
        axios
          .delete(`http://localhost:5000/api/Students/${this.studentToDelete}`)
          .then(() => {
            this.showSnackbar = true;
            this.snackbarMessage = "Student deleted successfully.";
            this.snackbarColor = "green";
            this.fetchStudents();
          })
          .catch((error) => {
            console.error(error);
            this.showSnackbar = true;
            this.snackbarMessage = "Error deleting student.";
            this.snackbarColor = "red";
          })
          .finally(() => {
            this.studentToDelete = null;
          });
      }
    },
  },
};

</script>

<style scoped>

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
.primary {
  background-color: #1976d2 !important;
}
.secondary {
  background-color: #424242 !important; 
}
.white--text {
  color: #ffffff !important;
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

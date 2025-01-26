<template>
  <v-container class="pa-6" fluid>
    <v-card elevation="12" class="professional-card">
      <v-card-title class="primary white--text">
        <span class="text-h5">{{ isEdit ? 'Edit Student' : 'Create Student' }}</span>
      </v-card-title>
      <v-card-text>
        <v-form ref="form" v-model="valid" @submit.prevent="save" class="mt-4">
          <v-row>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="student.name"
                label="Name"
                :rules="nameRules"
                required
                placeholder="Enter the student's name"
                clearable
                outlined
                dense
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="student.email"
                label="Email"
                :rules="emailRules"
                required
                placeholder="Enter the student's email"
                clearable
                outlined
                dense
              />
            </v-col>
          </v-row>
          <v-row>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="student.ra"
                label="RA"
                :rules="raRules"
                :disabled="isEdit"
                required
                placeholder="Enter RA"
                clearable
                outlined
                dense
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-text-field
                v-model="student.cpf"
                label="CPF"
                :rules="cpfRules"
                :disabled="isEdit"
                required
                placeholder="Enter CPF"
                clearable
                outlined
                dense
                v-mask="'###.###.###-##'"
                :counter="14"
                @input="onCPFInput"
              />
            </v-col>
          </v-row>

          <v-divider class="my-6"></v-divider>

          <v-row>
            <v-col class="d-flex justify-end">
              <v-btn
                color="primary"
                :disabled="!valid"
                @click="save"
                class="mr-2"
                elevation="2"
              >
                Save
              </v-btn>
              <v-btn
                color="grey"
                @click="cancel"
                class="elevation-2"
              >
                Cancel
              </v-btn>
            </v-col>
          </v-row>
        </v-form>
      </v-card-text>
    </v-card>

    <v-dialog v-model="showSuccessDialog" max-width="400">
      <v-card>
        <v-card-title class="headline primary white--text">
          Success
        </v-card-title>
        <v-card-text class="grey--text text--darken-1">
          {{ isEdit ? 'Student updated successfully!' : 'Student created successfully!' }}
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="redirectToList">
            OK
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

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

export default {
  name: 'StudentForm',
  data() {
    return {
      student: {
        id: null,
        name: '',
        email: '',
        ra: '',
        cpf: ''
      },
      isEdit: false,
      showSnackbar: false,
      snackbarMessage: '',
      snackbarColor: '',
      showSuccessDialog: false,
      valid: false,
      nameRules: [
        v => !!v || 'Name is required',
        v => (v && v.length <= 50) || 'Name must be less than 50 characters'
      ],
      emailRules: [
        v => !!v || 'Email is required',
        v => /.+@.+\..+/.test(v) || 'Email must be valid'
      ],
      raRules: [
        v => !!v || 'RA is required',
        v => /^[0-9]+$/.test(v) || 'RA must be numeric',
        v => (v && v.length <= 10) || 'RA must be less than 10 digits'
      ],
      cpfRules: [
        v => !!v || 'CPF is required',
        v => /^[0-9]{3}\.[0-9]{3}\.[0-9]{3}-[0-9]{2}$/.test(v) || 'CPF must be valid (999.999.999-99)'
      ]
    };
  },
  created() {
    if (this.$route.params.id) {
      this.isEdit = true;
      this.loadStudent(this.$route.params.id);
    }
  },
  methods: {
  
    loadStudent(id) {
      axios.get(`http://localhost:5000/api/Students/${id}`)
        .then(response => {
          const data = response.data;
          this.student = {
            ...data,
            cpf: this.formatCPF(data.cpf || '')
          };
        })
        .catch(error => {
          console.error(error);
          this.showSnackbar = true;
          this.snackbarMessage = 'Error loading student data.';
          this.snackbarColor = 'red';
        });
    },

  
    save() {
      if (this.$refs.form.validate()) {
        const payload = {
          id: this.student.id,
          name: this.student.name,
          email: this.student.email,
          ra: this.student.ra,
          cpf: this.unmaskCPF(this.student.cpf)
        };

        if (this.isEdit) {
          axios.put(`http://localhost:5000/api/Students/${this.student.id}`, payload)
            .then(() => {
              this.showSuccessDialog = true;
            })
            .catch(err => {
              console.error(err);
              this.showSnackbar = true;
              this.snackbarMessage = 'Error updating student.';
              this.snackbarColor = 'red';
            });
        } else {
          axios.post('http://localhost:5000/api/Students', payload)
            .then(() => {
              this.showSuccessDialog = true;
            })
            .catch(err => {
              console.error(err);
              this.showSnackbar = true;
              this.snackbarMessage = 'Error creating student.';
              this.snackbarColor = 'red';
            });
        }
      }
    },

   
    cancel() {
      this.$router.push({ name: 'StudentsList' });
    },

    
    redirectToList() {
      this.showSuccessDialog = false;
      this.$router.push({ name: 'StudentsList' });
    },

   
    unmaskCPF(cpf) {
      return cpf.replace(/\D/g, '');
    },

   
    formatCPF(value) {
      let cpf = value.replace(/\D/g, '');      
      cpf = cpf.substring(0, 11);             

      if (cpf.length > 9) {
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
      } else if (cpf.length > 6) {
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{0,3})/, '$1.$2.$3');
      } else if (cpf.length > 3) {
        cpf = cpf.replace(/(\d{3})(\d{0,3})/, '$1.$2');
      }

      return cpf;
    },

    
    onCPFInput() {
      this.student.cpf = this.formatCPF(this.student.cpf);
    }
  }
};
</script>

<style scoped>
.pa-6 {
  padding: 2rem !important;
}

.professional-card {
  border-radius: 12px;
}

.primary {
  background-color: #1976D2 !important; 
}

.white--text {
  color: white !important;
}

.v-btn {
  min-width: 120px;
}

.v-btn.primary {
  background-color: #1976D2;
}

.v-btn.grey {
  background-color: #B0BEC5;
}

.v-divider {
  border-top: 2px solid #E0E0E0;
}

@media (max-width: 600px) {
  .v-card-title {
    text-align: center;
  }

  .v-btn {
    width: 100%;
    margin-top: 10px;
  }

  .d-flex {
    flex-direction: column;
    align-items: stretch;
  }
}
</style>

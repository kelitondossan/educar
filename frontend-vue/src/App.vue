<template>
  <v-app>
    <!-- App Bar -->
    <v-app-bar app color="primary" dark>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>
      <v-toolbar-title>Welcome to the School Administration Environment</v-toolbar-title>
    </v-app-bar>

    <v-navigation-drawer v-model="drawer" app>
      <v-list dense>
        <v-list-item :to="{ name: 'Home' }" exact link>
          <v-icon left>mdi-home</v-icon>
          <v-list-item-title>Home</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'StudentsList' }" exact link>
          <v-icon left>mdi-account-group</v-icon>
          <v-list-item-title>Students List</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Dashboard' }" exact link>
          <v-icon left>mdi-view-dashboard</v-icon>
          <v-list-item-title>Dashboard</v-list-item-title>
        </v-list-item>

        <v-list-item :to="{ name: 'Reports' }" exact link>
          <v-icon left>mdi-file-document</v-icon>
          <v-list-item-title>Reports</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <v-container fluid>
        <router-view />
      </v-container>
    </v-main>

    <v-footer app>
      <span class="mx-auto">&copy; 2025 AcademicApp</span>
    </v-footer>
  </v-app>
</template>

<script>
import { ref, onMounted, onBeforeUnmount } from 'vue';

export default {
  name: 'App',
  setup() {
    const drawer = ref(false); 
    const studentsGroup = ref(false); 
    const isMobile = ref(false); 

    const checkScreenSize = () => {
      isMobile.value = window.innerWidth < 600;
      drawer.value = !isMobile.value;
    };

    onMounted(() => {
      checkScreenSize();
      window.addEventListener('resize', checkScreenSize);
    });

    onBeforeUnmount(() => {
      window.removeEventListener('resize', checkScreenSize);
    });

    return {
      drawer,
      studentsGroup,
      isMobile,
    };
  },
};
</script>

<style>
</style>

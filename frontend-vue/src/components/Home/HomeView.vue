<template>
  <v-container fluid class="pa-0">
    <v-parallax
      src="" 
      height="600"
    >
      <v-row class="fill-height" align="center" justify="center">
        <v-col cols="12" class="text-center white--text">
         
          <h1 class="animated-text">Welcome to the School Administration Environment</h1>
          <h3 class="animated-text">Manage your school efficiently and intuitively.</h3>

        </v-col>
      </v-row>
    </v-parallax>
    
  </v-container>
</template>

<script>
import { VParallax, VImg } from 'vuetify/lib/components';

export default {
  name: 'HomeView',
  components: {
    VParallax,
    VImg
  }
};
</script>

<style scoped>
.fill-height {
  height: 100%;
}

.white--text {
  color: rgb(22, 19, 19);
}

.animated-text {
  opacity: 0;
  transform: translateY(20px);
  animation: fadeInUp 1s forwards;
}

.animated-text:nth-child(1) {
  animation-delay: 0.5s;
}

.animated-text:nth-child(2) {
  animation-delay: 1s;
}

.animated-logo {
  opacity: 0;
  transform: scale(0.8);
  animation: fadeInScale 1s forwards, float 3s ease-in-out infinite;
  animation-delay: 0.3s;
}

@keyframes fadeInUp {
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

@keyframes fadeInScale {
  to {
    opacity: 1;
    transform: scale(1);
  }
}

@keyframes float {
  0% {
    transform: translateY(0px) scale(1);
  }
  50% {
    transform: translateY(-10px) scale(1.05);
  }
  100% {
    transform: translateY(0px) scale(1);
  }
}

@media (max-width: 600px) {
  .v-parallax {
    height: 400px;
  }
  
  .animated-text {
    font-size: 1.5rem;
  }
  
  .animated-logo {
    max-width: 150px;
  }
}
</style>

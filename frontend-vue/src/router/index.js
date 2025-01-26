import { createRouter, createWebHistory } from 'vue-router';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: () => import('@/components/Home/HomeView.vue') 
  },
  {
    path: '/students',
    name: 'StudentsList',
    component: () => import('@/components/StudentList/StudentsList.vue') 
  },
  {
    path: '/students/create',
    name: 'CreateStudent',
    component: () => import('@/components/StudentForm/StudentForm.vue') 
  },
  {
    path: '/students/edit/:id',
    name: 'EditStudent',
    component: () => import('@/components/StudentForm/StudentForm.vue'), 
    props: true
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: () => import('@/components/Dashboard/Dashboard.vue') 
  },
  {
    path: '/reports',
    name: 'Reports',
    component: () => import('@/components/Reports/Reports.vue') 
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;

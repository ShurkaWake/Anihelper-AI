import { createRouter, createWebHistory } from 'vue-router'
import CardView from "@/views/CardView.vue";
import LoginCard from "@/components/auth/LoginCard.vue";
import RegisterCard from "@/components/auth/RegisterCard.vue";
import VideoCreateCard from "@/components/video/VideoCreateCard.vue";
import VideoListView from "@/views/VideoListView.vue";
import VideoViewCard from "@/components/video/VideoViewCard.vue";
import UserView from "@/views/UserView.vue";
import VideoUpdateCard from "@/components/video/VideoUpdateCard.vue";
import VideoProcessCard from "@/components/video/VideoProcessCard.vue";
import CategoryCreateCard from "@/components/category/CategoryCreateCard.vue";
import CategoryView from "@/views/CategoryView.vue";
import CategoryUpdateCard from "@/components/category/CategoryUpdateCard.vue";
import AboutView from "@/views/AboutView.vue";

const routes = [
  {
    path: '/',
    name: 'home',
    component: AboutView
  },
  {
    path: '/video',
    name: 'allVideo',
    component: VideoListView
  },
  {
    path: '/category',
    name: 'allCategories',
    component: CategoryView
  },
  {
    path: '/profile',
    name: 'userProfile',
    component: UserView
  },
  {
    path: '/card-page',
    component: CardView,
    children: [
      {
        path: "/login",
        component: LoginCard
      },
      {
        path: "/register",
        component: RegisterCard
      },
      {
        path: "/video/create",
        component: VideoCreateCard
      },
      {
        path: "/video/:id",
        component: VideoViewCard
      },
      {
        path: "/video/:id/edit",
        component: VideoUpdateCard
      },
      {
        path: "/video/:id/download",
        component: VideoProcessCard
      },
      {
        path: "/category/create",
        component: CategoryCreateCard
      },
      {
        path: "/category/:id/edit",
        component: CategoryUpdateCard
      },
    ]
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

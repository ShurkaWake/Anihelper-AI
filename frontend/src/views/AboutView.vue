<template>
  <v-container>
    <!-- Header Section -->
    <v-row class="text-center">
      <v-col cols="12">
        <h1>Anihelper AI</h1>
      </v-col>
    </v-row>

    <!-- Create Video Section -->
    <v-row>
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title>Create Video</v-card-title>
          <v-card-text>
            Welcome to our AI Video Generation platform, where creating captivating videos has never been easier. Our
            cutting-edge technology allows you to transform your ideas into stunning visuals with just a few clicks.
            Whether you're a seasoned professional or a complete novice, our intuitive interface and powerful AI tools
            will guide you through the entire process.
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Share Video Section -->
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title>Share Video</v-card-title>
          <v-card-text>
            Once your video is ready, sharing it with the world is seamless. Our platform is designed to help you reach
            your audience wherever they are. Instantly share a link of your videos to popular social media platforms
            or send them directly to your email subscribers.
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Process Video Section -->
      <v-col cols="12" md="4">
        <v-card>
          <v-card-title>Process Video</v-card-title>
          <v-card-text>
            Processing your video content has never been more efficient. Our advanced algorithms ensure that your
            videos are rendered quickly without compromising quality. You can edit, enhance, and reprocess your videos
            effortlessly, allowing for continuous improvement and iteration. With secure cloud storage, your projects
            are always accessible and protected, giving you peace of mind as you create and share your video
            masterpieces. Join us today and experience the future of video creation, sharing, and processing.
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: 'AI Video Generation'
}
</script>

<style scoped>
h1 {
  margin-bottom: 40px;
}

v-card {
  margin: 20px 0;
}
</style>
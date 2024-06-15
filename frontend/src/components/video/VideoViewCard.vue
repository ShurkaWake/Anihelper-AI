<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";
import {mapState} from "vuex";

export default defineComponent({
  data: () => ({
    video: {},
    isLiked: false,
    creatorId: "",
    dialog: false,
    categoryName: ""
  }),

  computed: {
    ...mapState({
      role: state => state.auth.userRole,
      userId: state => state.auth.userId,
      userName: state => state.auth.username,
    }),

    isAdmin(){
      return this.role === "admin"
    },

    isOwner(){
      if (this.video === null){
        return false
      }
      return this.creatorId === this.userId
    },

    updateRoute(){
      return "/video/" + this.$route.params.id + "/edit"
    },

    processRoute(){
      return "/video/" + this.$route.params.id + "/download"
    }
  },

  methods: {
    async fetchVideo(){
      const videoId = this.$route.params.id;
      await axios.get('video/' + videoId)
          .then(response => {
            this.video = response.data.data
            this.isLiked = this.video.isLiked
            this.creatorId = this.video.creator.id
            this.categoryName = this.video.category.name
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "/")
            store.commit('switchDialog')
          })
    },

    async likeVideo(){
      await axios.post('video/' + this.video.id + '/like')
          .then(response => {
            this.video.likesCount += 1
            this.isLiked = true
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
    },

    async unlikeVideo(){
      await axios.post('video/' + this.video.id + '/unlike')
          .then(response => {
            this.video.likesCount -= 1
            this.isLiked = false
          })
          .catch(response => {
            store.commit('setErrorMessage', response.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
    },

    async deleteVideo(){
      this.dialog = false;
      await axios.delete("video/" + this.video.id)
          .then(response => {
            store.commit('setErrorMessage', "Video was successfully deleted");
            store.commit('setRedirectPath', "/video")
            store.commit('switchDialog')
          })
          .catch(error => {
            store.commit('setErrorMessage', error.response.data.errors.join("\n"));
            store.commit('setRedirectPath', "")
            store.commit('switchDialog')
          })
    },

    openDialog(){
      this.dialog = true;
    },
  },

  async mounted(){
    await this.fetchVideo()
  }
})
</script>

<template>
  <v-card class="pa-4 " width="500">
    <v-row class="justify-space-between ma-2">
      <div class="text-h5 ma-2">
        {{video.tittle}}
      </div>
      <v-btn density="default" icon="mdi-thumb-up" color="light-blue-darken-4" v-if="isLiked" @click="unlikeVideo"></v-btn>
      <v-btn density="default" icon="mdi-thumb-up" v-if="!isLiked" @click="likeVideo"></v-btn>
    </v-row>

    <v-divider/>
    <v-row class="justify-space-between">
      <v-col cols="10">
        <div class="model-fields ma-2 mt-3">
          Prompt: {{video.prompt}}
        </div>
        <div class="model-fields ma-2 mt-3">
          Resolution: {{video.height}}x{{video.width}}
        </div>
        <v-row class="model-fields ma-2 mt-3">
          <div class="model-fields">
            Duration: {{video.seconds}} s
          </div>
          <div class="model-fields ml-6">
            {{video.fps}} fps
          </div>
        </v-row>
        <div class="model-fields ma-2 mt-3">
          Likes: {{video.likesCount}}
        </div>
      </v-col>
      <v-col>
        <router-link
            :to="updateRoute"
            class="text-decoration-none"
            v-if="isOwner"
        >
          <v-btn
              density="default"
              icon="mdi-pencil"
              @click=""
              class="mt-3">
          </v-btn>
        </router-link>
        <v-btn
            density="default"
            icon="mdi-delete"
            @click="openDialog"
            class="mt-3"
            v-if="isOwner || isAdmin"
            color="red-darken-2">
        </v-btn>
        <router-link
            :to="processRoute"
            class="text-decoration-none">
          <v-btn
              density="default"
              icon="mdi-download"
              @click="" class="mt-3"
              color="green-darken-2">
          </v-btn>
        </router-link>
      </v-col>
    </v-row>
    <v-card
        prepend-icon="mdi-tag"
        color="blue-grey-lighten-5"
        class="pa-2 mt-8"
        v-bind:title='categoryName'
    />
    <v-card
        prepend-icon="mdi-account"
        color="blue-grey-lighten-5"
        class="pa-2 mt-8"
        v-bind:title='userName'
    />
  </v-card>

  <v-dialog
      v-model="dialog"
      max-width="400"
      persistent
  >

    <v-card
        prepend-icon="mdi-delete"
        text="Are you sure you want to delete the video? This action is irreversible"
        title="Confirm action"
    >
      <template v-slot:actions>
        <v-spacer></v-spacer>

        <v-btn @click="deleteVideo" color="red-darken-2">
          Delete
        </v-btn>

        <v-btn @click="dialog = false">
          Cancel
        </v-btn>
      </template>
    </v-card>
  </v-dialog>
</template>

<style>
.model-fields{
  font-size: 18px;
}
</style>
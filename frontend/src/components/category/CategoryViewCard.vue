<script>
import {defineComponent} from "vue";
import axios from "axios";
import store from "@/store";

export default defineComponent({
  data: () => ({
    dialog: false
  }),

  computed: {
    updateRoute(){
      return "/category/" + this.category.id + "/edit"
    },
  },

  methods: {
    async deleteCategory(){
      this.dialog = false;
      await axios.delete("category/" + this.category.id)
          .then(response => {
            this.$emit("CategoryDeleted")
            store.commit('setErrorMessage', "Category was successfully deleted");
            store.commit('setRedirectPath', "/category")
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

  props: {
    category: {
      type: Object,
      required: true
    },
  }
})
</script>

<template>
  <v-card class="pa-4 " width="500">
    <v-row class="justify-space-between ma-2">
      <div class="text-h5 ma-2">
        {{category.name}}
      </div>
      <router-link
          :to="updateRoute"
          class="text-decoration-none"
      >
        <v-btn
            density="default"
            icon="mdi-pencil"
            class="mt-3">
        </v-btn>
      </router-link>
    </v-row>

    <v-divider/>
    <v-row class="justify-space-between">
      <v-col cols="10">
        <div class="model-fields ma-2 mt-3">
          {{category.description}}
        </div>
      </v-col>
      <v-col>

        <v-btn
            density="default"
            icon="mdi-delete"
            @click="openDialog"
            class="mt-3"
            color="red-darken-2">
        </v-btn>
      </v-col>
    </v-row>
  </v-card>

  <v-dialog
      v-model="dialog"
      max-width="400"
      persistent
  >

    <v-card
        prepend-icon="mdi-delete"
        text="Are you sure you want to delete this category? This action is irreversible"
        title="Confirm action"
    >
      <template v-slot:actions>
        <v-spacer></v-spacer>

        <v-btn @click="deleteCategory" color="red-darken-2">
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
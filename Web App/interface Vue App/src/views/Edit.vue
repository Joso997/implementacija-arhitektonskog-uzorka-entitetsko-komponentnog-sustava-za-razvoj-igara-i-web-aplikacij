<template>
  <div class="container">
    <img alt="Vue logo" src="../assets/logo.png">
    <FormComponent msg="Welcome to Your Vue.js + TypeScript App"/>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import FormComponent from '@/components/FormComponent.vue' // @ is an alias to /src

@Options({
  components: {
    FormComponent
  }
})
export default class Edit extends Vue {}
</script>

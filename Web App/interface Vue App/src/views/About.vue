<template>
  <div class="container">
    <img alt="Vue logo" src="../assets/logo.png">
    <TableComponent msg="Welcome to Your Vue.js + TypeScript App"/>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import TableComponent from '@/components/TableComponent.vue' // @ is an alias to /src

@Options({
  components: {
    TableComponent
  }
})
export default class About extends Vue {}
</script>

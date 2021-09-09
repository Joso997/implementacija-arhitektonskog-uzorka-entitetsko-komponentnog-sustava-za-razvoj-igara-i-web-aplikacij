<template>
  <div v-if="$route.params.id !== undefined" class="mb-5 mt-0"><component :is="getComponent(constEntity.Region, constEntity.ObjectEnum)" :object='constEntity' @click="upgrade()"></component></div>
  <form v-if="renderComponent">
    <component  v-for="(_objectTemplate, key, index) in objectTemplates" :key="`${ key }-${ index }`" :is="getComponent(_objectTemplate.Region, _objectTemplate.ObjectEnum)" :object='_objectTemplate'> </component>
  </form>

</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { ObjectTemplate } from '@/interface/manager/containerClasses/objectTemplate'
import { Manager } from '@/interface/manager/mechanics/formMechanic'
import { MechanicAbstract } from '@/interface/manager/mechanics/mechanicAbstract'
import { RegionEnum, ObjectTypeEnum, SubObjectTypeEnum, ActionTypeEnum, StatTypeEnum, StatType, ObjectType, RegionType } from '@/interface/manager/events/types/index'
@Options({
  props: {
    msg: String
  }
})
export default class FormComponent extends Vue {
  msg!: string
  mechanic: MechanicAbstract = new Manager.Mechanic.FormMechanic()
  renderComponent= false
  objectTemplates!: ObjectTemplate[]
  constEntity = new ObjectTemplate(RegionEnum.Form, ObjectTypeEnum.Button, SubObjectTypeEnum.Right, ActionTypeEnum.Click, {
    [StatTypeEnum.Title]: StatType.StatTypes[StatTypeEnum.Title]().CreateStat().InitData('Nadogradi Formu'),
    [StatTypeEnum.Value]: StatType.StatTypes[StatTypeEnum.Value]().CreateStat(),
    [StatTypeEnum.Design]: StatType.StatTypes[StatTypeEnum.Design]().CreateStat().InitData('btn btn-outline-primary'),
    [StatTypeEnum.Label]: StatType.StatTypes[StatTypeEnum.Label]().CreateStat().InitData('AddParagraph')
  })

  beforeUnmount () {
    this.mechanic.UnsubscribeConditions()
  }

  created () {
    this.Init()
  }

  async Init () {
    this.objectTemplates = this.mechanic.InitSet(await this.mechanic.InitGet(this.$route.params.id === undefined ? -1 : Number(this.$route.params.id)))
    this.renderComponent = true
  }

  getComponent (_regionEnum : number, _objectEnum: number) {
    return RegionType.RegionTypes[_regionEnum].ObjectTypes[_objectEnum].GetVueComponent()
  }

  upgrade () {
    this.renderComponent = false
    this.objectTemplates = this.mechanic.ObjectTemplates
    this.renderComponent = true
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

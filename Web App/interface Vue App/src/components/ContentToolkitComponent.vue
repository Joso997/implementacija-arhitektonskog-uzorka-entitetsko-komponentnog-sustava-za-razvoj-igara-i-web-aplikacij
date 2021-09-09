<template>
  <table class="table" v-if="renderComponent">
  <thead>
    <tr>
      <th scope="col">Paragraf</th>
      <th scope="col"><component :is="getComponent(constEntity.Region, constEntity.ObjectEnum)" :object='constEntity' @click="add()"></component></th>
    </tr>
  </thead>
  <tbody v-if="objectTemplates !== undefined && objectTemplates.length > 0">
    <component  v-for="(_objectTemplate, key, index) in objectTemplates" :key="`${ key }-${ index }`" :is="getComponent(_objectTemplate.Region, _objectTemplate.ObjectEnum)" :object='_objectTemplate' :index='key'></component>
  </tbody>
</table>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component'
import { ObjectTemplate } from '@/interface/manager/containerClasses/objectTemplate'
import { Manager } from '@/interface/manager/mechanics/contentToolkitMechanic'
import { MechanicAbstract } from '@/interface/manager/mechanics/mechanicAbstract'
import { RegionEnum, ObjectTypeEnum, SubObjectTypeEnum, ActionTypeEnum, StatTypeEnum, StatType, ObjectType, RegionType } from '@/interface/manager/events/types/index'
@Options({
  props: {
    object: ObjectTemplate
  }
})
export default class TableComponent extends Vue {
  msg!: string
  mechanic: MechanicAbstract = new Manager.Mechanic.ContentToolkitMechanic()
  renderComponent= false
  objectTemplates!: ObjectTemplate[]
  statTypeEnum = StatTypeEnum
  objectTypeEnum = ObjectTypeEnum
  objectType = ObjectType
  regionType = RegionType
  regionEnum = RegionEnum
  object!: ObjectTemplate
  constEntity = new ObjectTemplate(RegionEnum.ContentToolkit, ObjectTypeEnum.Button, SubObjectTypeEnum.Right, ActionTypeEnum.Click, {
    [StatTypeEnum.Title]: StatType.StatTypes[StatTypeEnum.Title]().CreateStat().InitData('Dodaj Paragraf'),
    [StatTypeEnum.Value]: StatType.StatTypes[StatTypeEnum.Value]().CreateStat(),
    [StatTypeEnum.Design]: StatType.StatTypes[StatTypeEnum.Design]().CreateStat().InitData('btn btn-outline-primary'),
    [StatTypeEnum.Label]: StatType.StatTypes[StatTypeEnum.Label]().CreateStat().InitData('AddParagraph'),
    [StatTypeEnum.Id]: StatType.StatTypes[StatTypeEnum.Id]().CreateStat().InitData(this.object.Stats[StatTypeEnum.Id].Data)
  })

  beforeUnmount () {
    this.mechanic.UnsubscribeConditions()
  }

  created () {
    this.Init()
  }

  async Init () {
    // this.objectTemplates = this.mechanic.InitSet(await this.mechanic.InitGet(-1))
    this.renderComponent = true
  }

  add () {
    this.renderComponent = false
    const temp = [
      new ObjectTemplate(RegionEnum.Form, ObjectTypeEnum.ModularText, SubObjectTypeEnum.ParentObject, ActionTypeEnum.AppendEntity, {
        [StatTypeEnum.Title]: StatType.StatTypes[StatTypeEnum.Title]().CreateStat().InitData('Test'),
        [StatTypeEnum.Value]: StatType.StatTypes[StatTypeEnum.Value]().CreateStat().InitData(''),
        [StatTypeEnum.Design]: StatType.StatTypes[StatTypeEnum.Design]().CreateStat().InitData(''),
        [StatTypeEnum.Label]: StatType.StatTypes[StatTypeEnum.Label]().CreateStat().InitData('Content'),
        [StatTypeEnum.Id]: StatType.StatTypes[StatTypeEnum.Id]().CreateStat().InitData(this.object.Stats[StatTypeEnum.Id].Data)
      })
    ]
    if (this.objectTemplates !== undefined && this.objectTemplates.length > 0) {
      this.objectTemplates = this.mechanic.InitSet(this.objectTemplates.concat(temp))
    } else {
      this.objectTemplates = this.mechanic.InitSet(temp)
    }
    this.renderComponent = true
  }

  getComponent (_regionEnum : number, _objectEnum: number) {
    return RegionType.RegionTypes[_regionEnum].ObjectTypes[_objectEnum].GetVueComponent()
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>

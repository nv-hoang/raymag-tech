<template>
  <draggable
    :list="items"
    item-key="Id"
    :animation="200"
    handle=".handle"
    :group="{ name: 'g1' }"
    ghost-class="drag-ghost"
  >
    <template #item="{ element }">
      <div class="pt-1">
        <div class="item flex items-center gap-2 border border-gray-300 bg-white p-2 rounded-lg" style="background-color: #f8fafc;">
          <div class="handle text-gray-400" style="cursor: move;">
            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24"><!-- Icon from Mono Icons by Mono - https://github.com/mono-company/mono-icons/blob/master/LICENSE.md --><path fill="currentColor" d="M8.5 7a2 2 0 1 0 0-4a2 2 0 0 0 0 4m0 7a2 2 0 1 0 0-4a2 2 0 0 0 0 4m2 5a2 2 0 1 1-4 0a2 2 0 0 1 4 0m5-12a2 2 0 1 0 0-4a2 2 0 0 0 0 4m2 5a2 2 0 1 1-4 0a2 2 0 0 1 4 0m-2 9a2 2 0 1 0 0-4a2 2 0 0 0 0 4"/></svg>
          </div>
          <div class="flex-1">
            <div v-if="editLink" class="hover:text-primary-500  hover:underline inline-block">
              <a :href="editLink.replace('_id_', element.Id)" v-html="element.Name"></a>
            </div>
            <div v-else v-html="element.Name"></div>
          </div>
          <div v-if="element.Children && element.Children.length" class="cursor-pointer" @click="element.expand = (element.expand == undefined ? false:!element.expand)">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" aria-hidden="true" data-slot="icon" :class="element.expand == false ? 'size-4 transition-transform shrink-0' : 'size-4 transition-transform shrink-0 rotate-90'"><path stroke-linecap="round" stroke-linejoin="round" d="m8.25 4.5 7.5 7.5-7.5 7.5"></path></svg>
          </div>
        </div>
        <div style="padding-left: 30px;" v-if="element.expand == undefined || element.expand == true">
          <ui-nested :items="element.Children" :edit-link="editLink"></ui-nested>
        </div>
      </div>
    </template>
  </draggable>
</template>

<script>
import draggable from 'vuedraggable';

export default {
  components: {
    draggable,
  },
  props: {
    items: { type: Array, default: [] },
    editLink: String,
  },
  data() {
    return {
      drag: false,
    }
  },
}
</script>

<style>
.drag-ghost .item{
  border: 1px dashed var(--color-primary-500) !important;
}
</style>

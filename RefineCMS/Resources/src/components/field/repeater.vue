<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <div>
      <div v-if="layout == 'table'" class="overflow-x-auto">
        <table class="w-full">
          <thead>
            <tr>
              <th class="p-2 border border-r-0 border-gray-200 w-[30px]"></th>
              <th class="border border-l-0 border-gray-200 font-normal text-left min-w-[150px]">
                <div class="grid w-full" :style="colsize ? `grid-template-columns: ${colsize};` : 'grid-template-columns: repeat('+$slots.default().length+', minmax(0, 1fr));'">
                  <div v-for="vnode in $slots.default()" class="p-2 border-l border-gray-200">{{ vnode.props.label }}</div>
                </div>
              </th>
              <th class="p-2 border border-gray-200 w-[37px]"></th>
            </tr>
          </thead>
          <tbody v-if="show">
            <tr v-for="(item, idx) in items" class="hover-item">
              <td :class="[active == idx ? 'border-l-primary-500 border-l-2':'']" class="w-[30px] text-center bg-gray-50 p-2 border border-r-0 border-gray-200 text-gray-400 relative transition-all">
                <span>{{ idx + 1 }}</span>
  
                <div @click="addRow(idx)" class="item absolute -top-[16px] left-[calc(50%_-_10px)] text-[20px] text-primary-500 cursor-pointer">
                  <PlusCircleFilled></PlusCircleFilled>
                </div>
              </td>
              <td class="border border-l-0 border-gray-200">
                <component :is="colGroup" :fvalue="items[idx]" @field-change="(val) => items.splice(idx, 1, val)" @field-mounted="(field) => fields.splice(idx, 1, field)" :colsize="colsize"></component>
              </td>
              <td class="w-[30px] text-center bg-gray-50 border border-gray-200">
                <div @click="removeRow(idx)" class="cursor-pointer inline-block text-[20px] text-gray-400 hover:text-primary-500">
                  <CloseCircleOutlined></CloseCircleOutlined>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <div v-else>
        <table class="w-full">
          <tbody v-if="show">
            <tr v-for="(item, idx) in items" class="hover-item">
              <td :class="[active == idx ? 'border-l-primary-500 border-l-2':'']" class="w-[30px] text-center bg-gray-50 p-2 border border-gray-200 text-gray-400 relative transition-all">
                <span>{{ idx + 1 }}</span>
  
                <div @click="addRow(idx)" class="item absolute -top-[16px] left-[calc(50%_-_10px)] text-[20px] text-primary-500 cursor-pointer">
                  <PlusCircleFilled></PlusCircleFilled>
                </div>
              </td>
              <td class="p-2 border border-gray-200 min-w-[150px]">
                <component :is="rowGroup" :fvalue="items[idx]" @field-change="(val) => items.splice(idx, 1, val)" @field-mounted="(field) => fields.splice(idx, 1, field)"></component>
              </td>
              <td class="w-[30px] text-center bg-gray-50 border border-gray-200">
                <div @click="removeRow(idx)" class="cursor-pointer inline-block text-[20px] text-gray-400 hover:text-primary-500">
                  <CloseCircleOutlined></CloseCircleOutlined>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
  
      <div class="my-2">
        <a-button size="small" @click="addRow()">{{ $t('repeater.addRow') }}</a-button>
      </div>
    </div>
  </a-form-item>
</template>

<script>
import { h, resolveComponent } from 'vue';
import base from './base';

export default {
  mixins: [base],
  props: {
    layout: { type: String, default: 'table' },
    colsize: String,
  },
  data() {
    return {
      items: Array.isArray(this.fvalue) ? this.fvalue : [],
      fields: [],
      show: true,
      colGroup: h(resolveComponent('field-col-group'), { name: 'colgroup' }, { default: this.$slots.default }),
      rowGroup: h(resolveComponent('field-row-group'), { name: 'rowgroup' }, { default: this.$slots.default }),
      activeCtrl: null,
      active: null,
    };
  },
  watch: {
    fvalue(val) {
      this.items = Array.isArray(val) ? val : [];
      this.change(this.items);
    },
    fields(val) {
      this.$emit('field-mounted', val);
    },
  },
  methods: {
    isEmpty() {
      return this.items.length == 0;
    },
    debounce() {
      let scrollTop = window.scrollY;
      this.show = false;
      this.$nextTick(() => {
        this.show = true;
        this.$nextTick(() => window.scrollTo(0, scrollTop));
      });
    },
    addRow(idx) {
      var row = {};
      if(this.$slots.default) {
        for(const vnode of this.$slots.default()) {
          if(vnode.props.fdefault) {
            row[vnode.props.name] = vnode.props.fdefault;
          }
        }
      }

      if (idx === undefined) {
        this.active = this.items.length;
        this.items.push(row);
      } else {
        this.active = idx;
        this.items.splice(idx, 0, row);
      }
      
      if(this.activeCtrl) {
        clearTimeout(this.activeCtrl);
      }
      this.activeCtrl = setTimeout(() => {
        this.active = null;
        this.activeCtrl = null;
      }, 5000);
      
      this.debounce();
    },
    removeRow(idx) {
      this.items.splice(idx, 1);
      this.fields.splice(idx, 1);
      this.debounce();
    }
  },
  mounted() {
    this.$nextTick(() => {
      this.change(this.items);
      this.$emit('field-mounted', this.fields);
    });
  }
}
</script>
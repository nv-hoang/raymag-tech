<template>
  <a-form-item :help="getMessage()" :validate-status="getMessage() ? 'error':''">
    <a-select 
        show-search 
        v-model:value="searchValue"
        :disabled="disabled" 
        :placeholder="placeholder" 
        :default-active-first-option="false" 
        :show-arrow="false"
        :filter-option="false"
        :options="rows"
        :not-found-content="loading ? undefined : null"
        @search="fetch"
        @change="chooseOption"
        class="w-full"
    >
        <template v-if="loading" #notFoundContent>
            <a-spin size="small"></a-spin>
        </template>
    </a-select>

    <div class="mt-2 inline-block">
        <div v-for="(item, index) in selected" :class="['flex gap-2 p-1 rouned hover:bg-gray-50 border-gray-200', index > 0 ? 'border-t':'']">
            <div class="cursor-pointer hover:text-primary-500" @click="selected.splice(index, 1)">
              <CloseOutlined></CloseOutlined>
            </div>
            <component :is="item.label"></component>
        </div>
    </div>
  </a-form-item>
</template>

<script>
import { h } from 'vue';
import base from './base';
import qs from 'qs';

export default {
  mixins: [base],
  props: {
    apiuri: String,
  },
  data() {
    return {
        searchValue: '',
        rows: [],
        loading: false,
        selected: [],
    }
  },
  watch: {
    selected: {
      handler(newVal, oldVal) {
        var ids = this.selected.map(x => x.value);
        if(JSON.stringify(this.fvalue) != JSON.stringify(ids)) {
          this.change(ids);
        }
      },
      deep: true
    }
  },
  methods: {
    filterOption(input, option) {
      if(option.alt) {
        return option.alt.toLowerCase().includes(input.toLowerCase());;
      }
      return false;
    },
    fetch(search) {
        this.loading = true;
        this.api.get(this.apiuri, {
            params: {
                limit: 10,
                search: search,
            },
            paramsSerializer: params => qs.stringify(params, { arrayFormat: 'repeat', allowDots: true })
        }).then(res => {
            this.rows = res.data.posts.map(row => ({
                label: h('div', { innerHTML: row.Title }),
                value: row.Id,
            }))
        }).finally(() => {
            this.loading = false;
        });
    },
    chooseOption(id, item) {
        var idx = this.selected.findIndex(x => x.value == id);
        if(idx == -1) {
          this.selected.push(item);
        }
        this.searchValue = '';
    }
  },
  mounted() {
    if(this.$slots && this.$slots.default) {
      
      this.selected = this.$slots.default().map(x => ({
        value: x.props.value,
        label: h('div', x.children)
      }));
    }

    if(this.fvalue && this.fvalue.length > 0) {
      this.api.get(this.apiuri, {
          params: {
            ids: this.fvalue
          },
          paramsSerializer: params => qs.stringify(params, { arrayFormat: 'repeat', allowDots: true })
      }).then(res => {
          this.selected = res.data.posts.map(row => ({
              label: h('div', { innerHTML: row.Title }),
              value: row.Id,
          }));
      });
    }
  }
}
</script>
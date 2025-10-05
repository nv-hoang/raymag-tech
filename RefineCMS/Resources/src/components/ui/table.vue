<template>
  <div class="py-1 flex items-center gap-4">
    <div>
      <a-popconfirm :disabled="rowSelection.selectedRowKeys.length == 0" :title="$t('deleteConfirm.title')" :ok-text="$t('deleteConfirm.yes')" :cancel-text="$t('deleteConfirm.no')" @confirm="deleteRow()">
        <a-button type="primary" danger ghost :disabled="rowSelection.selectedRowKeys.length == 0">{{ $t('delete') }} <span v-if="rowSelection.selectedRowKeys.length">({{ rowSelection.selectedRowKeys.length }})</span></a-button>
      </a-popconfirm>
    </div>

    <div v-if="downloadfiles">
      <a-dropdown>
        <template #overlay>
          <a-menu>
            <a-menu-item v-for="filetype in downloadfiles.split(',')" :key="filetype">
              <a :href="GetDownloadLink(filetype)" target="_blank">{{ filetype }}</a>
            </a-menu-item>
          </a-menu>
        </template>
        <a-button>
          <span>{{ $t('download') }}</span>
          <DownOutlined></DownOutlined>
        </a-button>
      </a-dropdown>
    </div>

    <div class="flex-1"></div>
    <span v-show="total > 0" v-text="$t('total', [total])"></span>
  </div>
  <a-table class="ant-table-striped" ref="list" :row-class-name="striped" :loading="loading" :columns="columns" :row-key="record => record[rowKey]" :data-source="data" :row-selection="rowSelection" :pagination="false" :scroll="{ x: 1024 }" size="small"></a-table>
  <div class="my-4 text-center">
    <a-pagination v-model:current="current" :total="total" :defaultPageSize="pageSize" :showSizeChanger="false" :hideOnSinglePage="true" @change="(p) => fetch(p)"></a-pagination>
  </div>
</template>

<script>
import { h } from 'vue';
import qs from 'qs';

export default {
  props: {
    name: String,
    apiuri: String,
    rowKey: { type: String, default: 'Id' },
    selection: { type: Boolean, default: true },
    downloadfiles: String
  },
  data() {
    const colSize = {
      'Id': '50px',
      'Thumbnail': '100px',
      'PostTitle': '30%',
      'PostAuthor': '150px',
      'Status': '100px',
      'CreatedAt': '170px',
      'UpdatedAt': '170px',
    };

    const columns = this.$slots.default ? this.$slots.default().map(vnode => ({
      title: vnode.props.title,
      dataIndex: vnode.props.name,
      width: vnode.props.width ? `${vnode.props.width}px` : (colSize[vnode.props.name] ? colSize[vnode.props.name]:''),
      customRender: ({ record }) => {
        return h(vnode, { row: record });
      }
    })) : [];

    return {
      loading: false,
      search: '',
      filter: {},
      total: 0,
      current: 1,
      pageSize: 20,
      columns: columns,
      data: [],
      rowSelection: this.selection ? {
        selectedRowKeys: [],
        onChange: (selectedRowKeys, selectedRows) => {
          this.rowSelection.selectedRowKeys = selectedRowKeys;
        },
      } : null,
    }
  },
  methods: {
    striped(_record, index) {
      return index % 2 === 1 ? 'table-striped' : null;
    },
    fetch(page, deleteIds) {
      this.loading = true;
      this.current = page || 1;
      this.api.get(this.apiuri, {
        params: {
          skip: (this.current - 1) * this.pageSize,
          limit: this.pageSize,
          search: this.search,
          filter: this.filter,
          deleteIds: deleteIds ? deleteIds : [],
        },
        paramsSerializer: params => qs.stringify(params, { arrayFormat: 'repeat', allowDots: true })
      }).then(res => {
        this.loading = false;
        this.total = res.data.total;
        this.data = res.data.posts;
        this.rowSelection.selectedRowKeys = [];
      });
    },
    deleteRow() {
      if(this.rowSelection.selectedRowKeys.length) {
        this.fetch(null, this.rowSelection.selectedRowKeys);
      }
    },
    GetDownloadLink(filetype) {
      var params = {
        skip: 0,
        limit: -1,
        search: this.search,
        filter: this.filter,
        deleteIds: [],
      };
      return `${this.apiuri}-download/${filetype}?` + qs.stringify(params, { arrayFormat: 'repeat', allowDots: true });
    }
  },
  mounted() {
    this.fetch();
    
    this.emitter.on(this.name + '-search', (e) => {
      this.search = e;
      this.fetch();
    });
    
    this.emitter.on(this.name + '-filter', (e) => {
      this.filter = e;
      this.fetch();
    })

    this.emitter.on(this.name + '-clear', (e) => {
      this.search = '';
      this.filter = {};
      this.fetch();
    })
  }
}
</script>

<style>
.ant-table-striped:not(.ant-table-row-selected) .table-striped td {
  background: #fbfbfc;
}
</style>
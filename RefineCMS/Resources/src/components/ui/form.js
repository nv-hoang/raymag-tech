import { h, reactive, watch, onMounted, getCurrentInstance, nextTick } from 'vue';

export default {
  props: {
    name: { type: String, required: true },
  },
  setup(props, { slots }) {
    const instance = getCurrentInstance();
    const emitter = instance.appContext.config.globalProperties.emitter;

    var initdata = {};
    if (slots.default) {
      for (const vnode of slots.default()) {
        if (vnode.props.name == 'FormData' && vnode.children) {
          initdata = JSON.parse(vnode.children);
          break;
        }
      }
    }

    const formdata = reactive(initdata);
    const fields = reactive({});

    const loadData = (fieldData) => {
      for (const field in fieldData) {
        formdata[field] = fieldData[field];
      }
    }

    watch(formdata, (value) => {
      console.log('[Data]', value);
    }, { deep: true });

    onMounted(() => {
      emitter.on(props.name + '-load', loadData);
    });

    function valid(e, items) {
      if (items.checkValid) {
        items.checkValid(e);
      } else {
        for (var fname in items) {
          valid(e, items[fname])
        }
      }
    }

    return () => h(
      'form',
      {
        onSubmit: (e) => valid(e, fields),
      },
      slots.default ? slots.default(formdata).map(vnode => {
        if (vnode.props && vnode.props.name == 'FormData') {
          return h('textarea', {
            ...vnode.props,
            value: JSON.stringify(formdata),
          });
        } else if (vnode.type.name == 'form_actions') {
          return h(vnode);
        } else {
          return h(vnode, {
            fvalue: formdata[vnode.props.name] ?? vnode.props.fvalue,
            onFieldChange: (val) => (formdata[vnode.props.name] = val),
            onFieldMounted: (field) => (fields[vnode.props.name] = field),
          });
        }
      }) : ''
    );
  },
}
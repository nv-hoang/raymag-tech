import { h, resolveComponent, reactive, watch } from 'vue';
import base from './base';

export default {
    name: 'RowGroup',
    mixins: [base],
    setup(props, { slots, emit }) {
        const fdata = reactive(props.fvalue || {});
        const fields = reactive({});

        watch(fdata, (value) => {
            emit('field-change', fdata)
        }, { deep: true });

        watch(fields, (value) => {
            emit('field-mounted', fields)
        }, { deep: true });

        return () => h('div', { class: 'field-row-group border-l-2 md:border-0 border-gray-400' }, [
            slots.default ? slots.default().map(vnode => {
                return h(
                    resolveComponent('field-layout-row'),
                    { label: vnode.props.label, desc: vnode.props.desc, showif: vnode.props.showif, required: vnode.props.required },
                    {
                        default: () => h(vnode, {
                            fvalue: props.fvalue ? props.fvalue[vnode.props.name] : vnode.props.fvalue,
                            onFieldChange: (val) => (fdata[vnode.props.name] = val),
                            onFieldMounted: (field) => (fields[vnode.props.name] = field),
                        })
                    }
                );
            }) : null
        ]);
    }
}
import { h, resolveComponent, reactive, watch } from 'vue';
import base from './base';

export default {
    name: 'ColGroup',
    mixins: [base],
    props: {
        colsize: String,
    },
    setup(props, { slots, emit }) {
        const fdata = reactive(props.fvalue || {});
        const fields = reactive({});

        watch(fdata, (value) => {
            emit('field-change', fdata)
        }, { deep: true });

        watch(fields, (value) => {
            emit('field-mounted', fields)
        }, { deep: true });

        var cols = 1;
        if (slots.default) {
            cols = slots.default().length;
        }

        return () => h('div', { class: 'grid w-full', style: props.colsize ? `grid-template-columns: ${props.colsize};` : 'grid-template-columns: repeat(' + cols + ', minmax(0, 1fr));' }, [
            slots.default ? slots.default().map(vnode => {
                // return h(vnode, {
                //     class: 'border-l border-gray-200',
                //     style: 'padding: 8px !important;',
                //     fvalue: props.fvalue ? props.fvalue[vnode.props.name] : vnode.props.fvalue,
                //     onFieldChange: (val) => (fdata[vnode.props.name] = val),
                //     onFieldMounted: (field) => (fields[vnode.props.name] = field),
                // });
                return h('div',
                    {
                        class: 'border-l border-gray-200',
                        style: 'padding: 8px !important;',
                    },
                    h(vnode, {
                        fvalue: props.fvalue ? props.fvalue[vnode.props.name] : vnode.props.fvalue,
                        onFieldChange: (val) => (fdata[vnode.props.name] = val),
                        onFieldMounted: (field) => (fields[vnode.props.name] = field),
                    })
                );
            }) : null
        ]);
    }
}
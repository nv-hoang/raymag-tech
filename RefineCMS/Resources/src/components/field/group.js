import { h, resolveComponent, reactive, watch, ref } from 'vue';
import base from './base';

export default {
    mixins: [base],
    setup(props, { slots, emit }) {
        const fdata = reactive(props.fvalue || {});
        const fields = reactive({});
        const expand = ref(props.expand == 'true');

        watch(fdata, (value) => {
            emit('field-change', fdata);
        }, { deep: true });

        watch(fields, (value) => {
            emit('field-mounted', fields)
        }, { deep: true });

        return () => h('div', { class: 'field-group' }, [
            h('div', { class: 'flex items-center border-b-2 border-gray-400 pb-1.5 cursor-pointer select-none', onClick: () => expand.value = !expand.value }, [
                h('div', { class: 'flex-1' }, [
                    h('div', { class: 'text-[18px] font-medium text-gray-800' }, props.label),
                    props.desc ? h('div', props.desc) : null,
                ]),
                h('div', { class: 'text-[24px] text-gray-500 font-light' }, expand.value ? '−' : '+'),
            ]),
            h('div', { class: expand.value ? '' : 'hidden' }, slots.default ? slots.default().map(vnode => {
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
            }) : null)
        ]);
    }
}
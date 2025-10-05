import { h, resolveComponent, reactive, watch, getCurrentInstance } from 'vue';
import base from './base';

export default {
    name: 'LangGroup',
    mixins: [base],
    setup(props, { slots, emit }) {
        const fdata = reactive(props.fvalue || {});
        const fields = reactive({});

        const instance = getCurrentInstance();
        const api = instance.appContext.config.globalProperties.api;

        watch(fdata, (value) => {
            translate('Hello world', 'en', 'fr')
                .then(result => console.log('Translated:', result))
                .catch(err => console.error('Error:', err));
            emit('field-change', fdata)
        }, { deep: true });

        watch(fields, (value) => {
            emit('field-mounted', fields)
        }, { deep: true });

        const translate = (text, sourceLang = 'en', targetLang = 'es') => {
            return api.post('https://libretranslate.com/translate', {
                q: text,
                source: sourceLang,
                target: targetLang,
                format: 'text'
            }, {
                headers: {
                    'Content-Type': 'application/json'
                }
            }).then(response => {
                return response.data.translatedText;
            }).catch(error => {
                console.error('Translation error:', error.response?.data || error.message);
                throw error;
            });
        }

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
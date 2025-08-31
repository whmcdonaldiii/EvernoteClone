// Quill.js JavaScript Interop for Blazor
window.quillInterop = {
    quillInstances: {},

    // Initialize Quill editor
    initializeQuill: function (elementId, dotNetRef, initialContent) {
        try {
            const toolbarOptions = [
                ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
                ['blockquote', 'code-block'],
                
                [{ 'header': 1 }, { 'header': 2 }],               // custom button values
                [{ 'list': 'ordered'}, { 'list': 'bullet' }],
                [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
                [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
                [{ 'direction': 'rtl' }],                         // text direction
                
                [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
                [{ 'header': [1, 2, 3, 4, 5, 6, false] }],
                
                [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
                [{ 'font': [] }],
                [{ 'align': [] }],
                
                ['clean'],                                        // remove formatting button
                ['link', 'image']                                 // link and image, video
            ];

            const quill = new Quill(`#${elementId}`, {
                theme: 'snow',
                modules: {
                    toolbar: toolbarOptions
                },
                placeholder: 'Start writing your note...'
            });

            // Set initial content
            if (initialContent) {
                quill.setContents(quill.clipboard.convert(initialContent));
            }

            // Store the instance
            this.quillInstances[elementId] = {
                quill: quill,
                dotNetRef: dotNetRef
            };

            // Listen for text changes
            quill.on('text-change', () => {
                const html = quill.root.innerHTML;
                dotNetRef.invokeMethodAsync('OnContentChanged', html);
            });

            return true;
        } catch (error) {
            console.error('Error initializing Quill:', error);
            return false;
        }
    },

    // Get content from Quill editor
    getContent: function (elementId) {
        const instance = this.quillInstances[elementId];
        if (instance && instance.quill) {
            return instance.quill.root.innerHTML;
        }
        return '';
    },

    // Set content in Quill editor
    setContent: function (elementId, content) {
        const instance = this.quillInstances[elementId];
        if (instance && instance.quill) {
            if (content) {
                instance.quill.setContents(instance.quill.clipboard.convert(content));
            } else {
                instance.quill.setText('');
            }
        }
    },

    // Destroy Quill instance
    destroyQuill: function (elementId) {
        const instance = this.quillInstances[elementId];
        if (instance && instance.quill) {
            // Clean up any event listeners
            instance.quill.off('text-change');
            delete this.quillInstances[elementId];
        }
    },

    // Focus the editor
    focus: function (elementId) {
        const instance = this.quillInstances[elementId];
        if (instance && instance.quill) {
            instance.quill.focus();
        }
    }
};

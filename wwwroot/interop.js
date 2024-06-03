window.showFolderPicker = async () => {
    try {
        const pickerOpts = {
            types: [],
            excludeAcceptAllOption: true,
            multiple: false
        };

        const handle = await window.showDirectoryPicker(pickerOpts);
        return handle.name;
    } catch (err) {
        console.error(err);
        return null;
    }
};
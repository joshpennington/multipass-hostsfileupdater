# Multipass Hosts File Updater

This program will update your hosts file (Windows only for now) to map domains to the IP address of a specific Multipass instance.

To setup the mapping create a file in your Document folder called `.multipass-hosts`. The format of the file is as follows:

```json
[
    {
        "name": "primary",
        "uris": [
            "url1.local",
            "url2.local",
            "url3.local"
        ]
    }
]
```
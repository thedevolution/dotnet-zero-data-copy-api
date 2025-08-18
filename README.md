# snowflake-zero-data-copy
Full-stack developers intro to eliminating data duplication, reducing storage costs, and ensuring access of the most up-to-date information

## Snowflake

Link to DeepSync U.S. ZIP Code Metadata via Snowflake's Marketplace:
https://app.snowflake.com/marketplace/listing/GZT1ZJ0SBC/deep-sync-u-s-zip-code-metadata

Upon importing the marketplace data, need to grant privileges to your database role that is associated to your database user, something like the following command:

`GRANT IMPORTED PRIVILEGES ON DATABASE U_S__ZIP_CODE_METADATA TO ROLE MY_APPLICATION_ROLE;`

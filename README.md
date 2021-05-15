**Edit a file, create a new file, and clone from Bitbucket in under 2 minutes**

When you're done, you can delete the content in this README and update the file with details for others getting started
with your repository.

*We recommend that you open this README in another tab as you perform the tasks below. You
can [watch our video](https://youtu.be/0ocf7u76WSo) for a full demo of all the steps in this tutorial. Open the video in
a new tab to avoid leaving Bitbucket.*

---

## Paquetes del proyecto.

- [jdenticon](https://github.com/dmester/jdenticon) (library for generating identicons).

## Consulta Órdenes de reparación.

```
db.contacts.aggregate([
    {
        "$lookup": {
            "from": "order_repairs", "localField": "_id", "foreignField": "client_id", "as": "OrderRepair"
        }
    },
    {
        "$project": {
            "OrderRepair": {
                "$filter": {
                    "input": "$OrderRepair",
                    "as": "rep",
                    "cond": {
                        "$eq": ["$$rep.is_deleted", false]
                    }
                }
            }
        }
    },
    {
        "$unwind": "$OrderRepair"
    }
])
```
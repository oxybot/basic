import { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addWarning } from "../Alerts/slice";
import { apiFetch } from "../api";
import EntityForm from "./EntityForm";

const defaultTransform = (e) => e;
const defaultOnUpdate = () => {};
const defaultExtendedForm = () => null;

export default function PageEdit({
  definition,
  baseApiUrl,
  entityId,
  full = false,
  texts,
  onUpdate = defaultOnUpdate,
  transform = defaultTransform,
  extendedForm = defaultExtendedForm,
}) {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const [entity, setEntity] = useState({});
  const [errors, setErrors] = useState({});
  texts["form-action"] = "Update";

  useEffect(() => {
    apiFetch([baseApiUrl, entityId], { method: "GET" }).then((response) => {
      setEntity(transform(response));
    });
  }, [baseApiUrl, entityId, transform]);

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value === "" ? null : event.target.value;
    setEntity({ ...entity, [name]: value });
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    apiFetch([baseApiUrl, entityId], {
      method: "PUT",
      body: JSON.stringify(entity),
    })
      .then(() => {
        navigate("./..");
        onUpdate();
      })
      .catch((err) => {
        if (typeof err === "string") {
          dispatch(addWarning("Invalid information", "Please review the values provided"));
        } else {
          setErrors(err);
        }
      });
  };

  return (
    <EntityForm
      definition={definition}
      entity={entity}
      full={full}
      texts={texts}
      errors={errors}
      handleChange={handleChange}
      handleSubmit={handleSubmit}
    >
      {extendedForm(entity, setEntity, errors)}
    </EntityForm>
  );
}

import React from 'react';
import "./Popover.css";
import Button from 'react-bootstrap/Button';
import Popover from 'react-bootstrap/Popover';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';


const styles = {
  occurrence: {
    color: 'rgba(255, 146, 45, 1.0)',
    //orange
  },
  type: {
      color: 'rgba(255, 193, 7, 1.0)',
      //yellow
  },
  negation: {
      color: 'rgba(67,205,49,1.0)',
      //apple
  },

  property: {
      color: 'rgba(40, 167, 69, 1.0)',
      //green
  },
  operation: {
      color: 'rgba(240,253,199, 1.0)',
      //mimosa
  },
  value: {
      color: 'rgba(20, 99, 112, 1.0)',
      //darker cyan
  },
  unit: {
      color: 'rgba(252,153,163, 1.0)',
      //Sweet Pink
  },
  logicalOperator: {
      color: 'rgba(198, 198, 255, 1.0)',
      //purple
  },
  relation: {
      color: 'rgba(0, 123, 255, 1.0)',
      //blue
  },

  unknown: {
      color: 'rgba(220, 53, 69, 1.0)',
      //red
  }
};

//Each of these functions determines the contents of each popover.
const popoverExample = (
    <Popover id="popover-basic">
      <Popover.Content>
      All <span style={styles.type}>windows</span> should have a <span style={styles.property}>width</span> of <span style={styles.unit}>no less than 15 inch</span> and a <span style={styles.property}>height</span> of <span style={styles.unit}>no more than 2 feet</span> and is <span style={styles.relation}>above</span> a <span style={styles.type}>sink</span>.
      </Popover.Content>
    </Popover>
);
const popoverOccurrence = (
  <Popover id="popover-basic">
    <Popover.Content>
    The occurrence of the object (ALL, ANY, NONE).
    </Popover.Content>
  </Popover>
);

const popoverType = (
    <Popover id="popover-basic">
      <Popover.Content>
      The type of the object (Chair, Refrigerator, etc.).
      </Popover.Content>
    </Popover>
);

const popoverNegation = (
  <Popover id="popover-basic">
    <Popover.Content>
    Negation (Not etc.)
    </Popover.Content>
  </Popover>
);

const popoverProperty = (
    <Popover id="popover-basic">
      <Popover.Content>
      Properties the object possesses (width, height, etc.)
      </Popover.Content>
    </Popover>
);

const popoverOperation = (
  <Popover id="popover-basic">
    <Popover.Content>
    Operation of the property or relation (less than, more than etc.)
    </Popover.Content>
  </Popover>
);

const popoverValue = (
  <Popover id="popover-basic">
    <Popover.Content>
    Numeric Value (0, 1, 2 etc.)
    </Popover.Content>
  </Popover>
);

const popoverUnit = (
  <Popover id="popover-basic">
    <Popover.Content>
    Units of measurement used (in, ft, m).
    </Popover.Content>
  </Popover>
);

const popoverLogicalOperator = (
  <Popover id="popover-basic">
    <Popover.Content>
    Logical Operator to connect check expressions (AND, OR, XOR).
    </Popover.Content>
  </Popover>
);

const popoverRelation = (
    <Popover id="popover-basic">
      <Popover.Content>
      The relationship between objects (directly against, above, below, between).
      </Popover.Content>
    </Popover>
);

const popoverUnknown = (
    <Popover id="popover-basic">
      <Popover.Content>
      Unrecognized component.
      </Popover.Content>
    </Popover>
);

export const popoverSuggestion = (
    <Popover id="popover-basic">
      <Popover.Content>
      Here some suggested words ;)
      </Popover.Content>
    </Popover>
);

//Buttons to display the popovers that were defined above. Buttons are called in App.js to be displayed.
export const ExampleButton = () => (
    <OverlayTrigger trigger="click" placement="top" overlay={popoverExample}>
      <Button variant="primary">Example</Button>
    </OverlayTrigger>
);

export const OccurrenceButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverOccurrence}>
    <Button /*variant="outline-success"*/ id="Occurrance_Button">Occurrence</Button>
  </OverlayTrigger>
);

export const TypeButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverType}>
      <Button variant="outline-warning">Type</Button>
    </OverlayTrigger>
);

export const NegationButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverProperty}>
    <Button /*variant="outline-success"*/ id="Negation_Button">Negation</Button>
  </OverlayTrigger>
);


export const PropertyButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverProperty}>
      <Button variant="outline-success">Property</Button>
    </OverlayTrigger>
);

export const OperationButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverProperty}>
    <Button /*variant="outline-success"*/ id="Operation_Button">Operation</Button>
  </OverlayTrigger>
);

export const ValueButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnit}>
    <Button /*variant="outline-info"*/ id="Value_Button">Value</Button>
  </OverlayTrigger>
);

export const UnitButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnit}>
    <Button variant="outline-info">Unit</Button>
  </OverlayTrigger>
);

export const LogicalOperatorButton = () => (
  <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnit}>
    <Button /*variant="outline-info"*/ id="LogicalOperator_Button">LogicalOperator</Button>
  </OverlayTrigger>
);

export const RelationButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverRelation}>
      <Button variant="outline-primary">Relation</Button>
    </OverlayTrigger>
);

export const UnknownButton = () => (
    <OverlayTrigger trigger={["hover", "focus"]} placement="top" overlay={popoverUnknown}>
      <Button variant="outline-danger">Unknown</Button>
    </OverlayTrigger>
);




